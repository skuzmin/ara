using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using ARA.Dialogs;
using ARA.Interfaces;
using ARA.Models;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
#pragma warning disable SYSLIB1054

namespace ARA.Services
{
	public class LoadoutCheckerService : ILoadoutCheckerService
	{
		#region WinAPI
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)] static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);
		[DllImport("user32.dll", SetLastError = true)] private static extern bool IsWindow(IntPtr hWnd);
		[DllImport("user32.dll", SetLastError = true)] private static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, uint nFlags);
		[DllImport("user32.dll", SetLastError = true)] private static extern bool IsWindowVisible(IntPtr hWnd);
		[DllImport("user32.dll", SetLastError = true)] private static extern bool IsIconic(IntPtr hWnd);
		[DllImport("user32.dll", SetLastError = true)] private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
		[StructLayout(LayoutKind.Sequential)] private struct RECT { public int Left, Top, Right, Bottom; }
		private const uint PW_CLIENTONLY = 0x00000001;
		private const uint PW_RENDERFULLCONTENT = 0x00000002;
		#endregion

		private const string WINDOW_PROC_NAME = "PioneerGame";
		private const double REF_HEIGHT = 1440;
		private const double REF_WIDTH = 2560;
		private const double REF_ICON_SIZE = 128;
		private const double REF_MASK_HEIGHT = 32;
		private const double REF_MASK_WITH = 40;
		private const double THRESHOLD_STAGE_1 = 0.8;
		private const double THRESHOLD_STAGE_2 = 0.9;
		private readonly ILogger _logger;
		private readonly IAraTranslation _translations;
		private IntPtr _hwnd;
		public LoadoutCheckerService(ILogger logger, IAraTranslation translations)
		{
			_logger = logger;
			_translations = translations;
		}

		public bool IsGameDetected()
		{
			return _hwnd != IntPtr.Zero;
		}

		public void CaptureGameWindow()
		{
			var proc = Process.GetProcessesByName(WINDOW_PROC_NAME);
			if (proc == null || proc.Length == 0)
			{
				_logger.LogError("Can't find process: {proc}", WINDOW_PROC_NAME);
				var dialogConfig = new ConfirmationDialogConfig
				{
					Title = _translations.Translate("GameNotification.NoFound.Title"),
					Message = _translations.Translate("GameNotification.NoFound.Message"),
					SubMessage = _translations.Translate("GameNotification.NoFound.SubMessage"),
					ConfirmButtonText = _translations.Translate("General.Confirmation.OK"),
				};
				new ConfirmationDialog(dialogConfig).ShowDialog();
				return;
			}
			_hwnd = proc[0].MainWindowHandle;
			_logger.LogInformation("PROC: {proc} | WIN: {win} | HWND: {hwnd}", proc, proc[0], _hwnd);
		}


		public Dictionary<int, bool> CheckIcons(List<GameItem> icons)
		{
			var results = new Dictionary<int, bool>();
			using var region = CaptureScreen();

			if (!IsGameDetected() || region == null || GetClientGameArea() is not { } size)
			{
				foreach (var item in icons)
				{
					results[item.Id] = false;
				}
				return results;
			}

			double ratioW = size.Width / REF_WIDTH;
			double ratioH = size.Height / REF_HEIGHT;
			double scale = Math.Abs(ratioH - 1) > Math.Abs(ratioW - 1) ? ratioH : ratioW;
			double iconSize = REF_ICON_SIZE * scale;

			var candidates = icons
				.Select(item => (icon: LoadTemplate(item.Path), item.Name, iconSize))
				.ToList();

			var matchResults = MatchAll(region, candidates);
			foreach (var item in icons)
			{
				results[item.Id] = matchResults.TryGetValue(item.Name, out bool matched) && matched;
			}

			foreach (var candidate in candidates)
			{
				candidate.icon.Dispose();
			}

			return results;
		}


		private System.Drawing.Size? GetClientGameArea()
		{
			System.Drawing.Size? result = null;
			if (!IsWindowVisible(_hwnd) || IsIconic(_hwnd))
			{
				_logger.LogError("Game window is unavailable (hidden or minimized)");
			}
			else if (!GetClientRect(_hwnd, out RECT rect))
			{
				_logger.LogError("GetClientRect: Game area was not found");
			}
			else
			{
				result = new System.Drawing.Size(rect.Right, rect.Bottom);
			}

			if (result == null)
			{
				var dialogConfig = new ConfirmationDialogConfig
				{
					Title = _translations.Translate("GameNotification.Minimized.Title"),
					Message = _translations.Translate("GameNotification.Minimized.Message"),
					SubMessage = _translations.Translate("GameNotification.Minimized.SubMessage"),
					ConfirmButtonText = _translations.Translate("General.Confirmation.OK"),
				};
				Application.Current.Dispatcher.Invoke(() => new ConfirmationDialog(dialogConfig).ShowDialog());
			}

			return result;
		}

		private Mat LoadTemplate(string packUri)
		{
			var uri = new Uri(packUri, UriKind.Absolute);
			var streamInfo = Application.GetResourceStream(uri);

			if (streamInfo == null)
			{
				_logger.LogError("Resource not found: {packUri}", packUri);
				return new Mat();
			}

			using var stream = streamInfo.Stream;
			using var memoryStream = new MemoryStream();

			stream.CopyTo(memoryStream);
			byte[] bytes = memoryStream.ToArray();

			return Mat.FromImageData(bytes, ImreadModes.Color);
		}

		private Dictionary<string, bool> MatchAll(Mat region, List<(Mat icon, string name, double iconSize)> icons)
		{
			using var regionGray = ToGray(region, blurSize: 3);
			var results = new ConcurrentDictionary<string, bool>();
			var stage2Candidates = new ConcurrentBag<(Mat resize, string name)>();
			// Stage 1: Fast check icons in parallel (Grayscale and Resize)
			Parallel.ForEach(icons, item =>
			{
				using var iconGray = ToGray(item.icon, blurSize: 5);
				using var resize = new Mat();
				Cv2.Resize(iconGray, resize, new OpenCvSharp.Size(item.iconSize, item.iconSize), interpolation: InterpolationFlags.Area);

				using var resultFast = new Mat();
				Cv2.MatchTemplate(regionGray, resize, resultFast, TemplateMatchModes.CCoeffNormed);
				Cv2.MinMaxLoc(resultFast, out _, out double fastVal, out _, out _);

				if (fastVal < THRESHOLD_STAGE_1)
				{
					results[item.name] = false;
				}
				else if (fastVal >= THRESHOLD_STAGE_2)
				{
					results[item.name] = true;
				}
				else
				{
					stage2Candidates.Add((resize.Clone(), item.name));
				}
				_logger.LogInformation("Stage #1 | [{icon} : {value}]", item.name, $"{fastVal:F3}");
			});

			if (stage2Candidates.IsEmpty)
			{
				return results.ToDictionary(k => k.Key, v => v.Value);
			}

			// Create mask using Icon scaled size to cover 40x32(ingame item quantity) in the bottom right corner
			var firstCandidate = stage2Candidates.First();
			var maskSize = firstCandidate.resize.Size();
			double scale = maskSize.Width / REF_ICON_SIZE;
			int maskW = (int)(REF_MASK_WITH * scale);
			int maskH = (int)(REF_MASK_HEIGHT * scale);

			using Mat sharedMask = Mat.Ones(maskSize, MatType.CV_8UC1) * 255;
			sharedMask[new OpenCvSharp.Rect(maskSize.Width - maskW, maskSize.Height - maskH, maskW, maskH)].SetTo(Scalar.Black);

			// Stage 2: Detailed check of candidate icons using mask
			Parallel.ForEach(stage2Candidates, candidate =>
			{
				using var resultMasked = new Mat();
				Cv2.MatchTemplate(regionGray, candidate.resize, resultMasked, TemplateMatchModes.CCoeffNormed, sharedMask);
				Cv2.MinMaxLoc(resultMasked, out _, out double maskedVal, out _, out _);
				results[candidate.name] = maskedVal >= THRESHOLD_STAGE_2;
				candidate.resize.Dispose();
				_logger.LogInformation("Stage #2 | [{icon} : {value}]", candidate.name, $"{maskedVal:F3}");
			});

			return results.ToDictionary(k => k.Key, v => v.Value);
		}

		private Mat ToGray(Mat mat, int blurSize)
		{
			if (mat.Empty())
			{
				_logger.LogError("ToGray: input Mat is empty");
				return new Mat();
			}

			Mat gray;
			switch (mat.Channels())
			{
				case 1: gray = mat.Clone(); break;
				case 3: gray = mat.CvtColor(ColorConversionCodes.BGR2GRAY); break;
				case 4: gray = mat.CvtColor(ColorConversionCodes.BGRA2GRAY); break;
				default:
					_logger.LogError("Unsupported channel count: {c}", mat.Channels());
					return new Mat();
			}
			Cv2.GaussianBlur(gray, gray, new OpenCvSharp.Size(blurSize, blurSize), 0);
			return gray;
		}

		private Mat? CaptureScreen()
		{
			if (!IsHandleValid() || GetClientGameArea() is not { } size)
			{
				return null;
			}

			using var bmp = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
			using Graphics g = Graphics.FromImage(bmp);
			IntPtr hdcDest = g.GetHdc();
			try
			{
				uint flags = PW_CLIENTONLY | PW_RENDERFULLCONTENT;
				bool success = PrintWindow(_hwnd, hdcDest, flags);

				if (!success)
				{
					_logger.LogError("PrintWindow failed. Win32 error: {err}", Marshal.GetLastWin32Error());
					return null;
				}
			}
			finally
			{
				g.ReleaseHdc(hdcDest);
			}

			using var full = BitmapConverter.ToMat(bmp);
			int halWidth = full.Width / 2;
			var roi = new OpenCvSharp.Rect(halWidth, 0, halWidth, full.Height);
			return full[roi].Clone();
		}

		private bool IsHandleValid()
		{
			var result = true;
			if (!IsGameDetected())
			{
				_logger.LogError("Window handle is null.");
				result = false;
			}

			if (!IsWindow(_hwnd))
			{
				_logger.LogError("The provided handle is no longer a valid window.");
				_hwnd = IntPtr.Zero;
				result = false;
			}
			if (!result)
			{
				var dialogConfig = new ConfirmationDialogConfig
				{
					Title = _translations.Translate("GameNotification.Closed.Title"),
					Message = _translations.Translate("GameNotification.Closed.Message"),
					SubMessage = _translations.Translate("GameNotification.Closed.SubMessage"),
					ConfirmButtonText = _translations.Translate("General.Confirmation.OK"),
				};
				Application.Current.Dispatcher.Invoke(() =>
				{
					new ConfirmationDialog(dialogConfig).ShowDialog();
				});
			}
			return result;
		}
	}
}
