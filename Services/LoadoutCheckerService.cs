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

// dotnet publish -c Release
// add detect window button in settings, bug during save settings when app is closed/minimized
// add read.me

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

		//private const string WINDOW_PROC_NAME = "PioneerGame";
		private const string WINDOW_PROC_NAME = "notepad";
		private const double REF_HEIGHT = 1440;
		private const double REF_WIDTH = 2560;
		private const double REF_ICON_SIZE = 128;
		private const double THRESHOLD = 0.9;
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

		public void InitGameWindow()
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

		public Dictionary<int, bool> CheckIcons(int x, int y, int width, int height, List<GameItem> icons)
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
			double changeH = Math.Abs(ratioH - 1);
			double changeW = Math.Abs(ratioW - 1);
			double scale = changeH > changeW ? ratioH : ratioW;
			double iconSize = REF_ICON_SIZE * scale;

			foreach (var item in icons)
			{
				using var template = LoadTemplate(item.Path);
				results[item.Id] = MatchSingle(region, template, item.Name, iconSize);
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
				Application.Current.Dispatcher.Invoke(() =>
				{
					new ConfirmationDialog(dialogConfig).ShowDialog();
				});
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

		private bool MatchSingle(Mat region, Mat icon, string name, double iconSize)
		{
			using var regionBgr = ToGray(region);
			using var iconBgr = ToGray(icon);
			if (regionBgr.Empty() || iconBgr.Empty())
			{
				return false;
			}

			using Mat result = new();
			using Mat resize = new();

			Cv2.Resize(iconBgr, resize, new OpenCvSharp.Size(iconSize, iconSize), interpolation: InterpolationFlags.Linear);
			Cv2.MatchTemplate(regionBgr, resize, result, TemplateMatchModes.CCoeffNormed);
			Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out _);

			_logger.LogInformation("Icon check [{icon} : {value}]", name, $"{maxVal:F3}");
			return maxVal >= THRESHOLD;
		}

		private Mat ToGray(Mat mat)
		{
			if (mat.Empty())
			{
				_logger.LogError("ToGray: input Mat is empty");
				return new Mat();
			}

			switch (mat.Channels())
			{
				case 1: return mat.Clone();
				case 3: return mat.CvtColor(ColorConversionCodes.BGR2GRAY);
				case 4: return mat.CvtColor(ColorConversionCodes.BGRA2GRAY);
				default:
					_logger.LogError("Unsupported channel count: {c}", mat.Channels());
					return new Mat();
			}
		}

		private Mat? CaptureScreen()
		{
			if (!IsHandleValid() || GetClientGameArea() is not { } size)
			{
				return null;
			}

			var bmp = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
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
