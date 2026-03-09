using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using ARA.Interfaces;
using ARA.Models;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace ARA.Services
{
	public class LoadoutCheckerService : ILoadoutCheckerService
	{
		private const double THRESHOLD = 0.8;
		private readonly ILogger _logger;
		public LoadoutCheckerService(ILogger logger)
		{
			_logger = logger;
		}

		public Dictionary<int, bool> CheckIcons(int x, int y, int width, int height, List<GameItem> icons)
		{
			using var region = CaptureRegion(x, y, width, height);

			var results = new Dictionary<int, bool>();

			foreach (var item in icons)
			{
				using var template = LoadTemplate(item.Path);
				results[item.Id] = MatchSingle(region, template, item.Name);
			}

			return results;
		}

		private Mat LoadTemplate(string packUri)
		{
			var uri = new Uri(packUri, UriKind.Absolute);
			var streamInfo = Application.GetResourceStream(uri);

			if (streamInfo == null)
			{
				_logger.LogError("Resource not found: {packUri}", packUri);
				throw new FileNotFoundException($"Resource not found: {packUri}");
			}

			using var stream = streamInfo.Stream;
			using var memoryStream = new MemoryStream();

			stream.CopyTo(memoryStream);
			byte[] bytes = memoryStream.ToArray();

			return Mat.FromImageData(bytes, ImreadModes.Color);
		}

		private bool MatchSingle(Mat region, Mat icon, string name)
		{
			using var regionBgr = ToGray(region);
			using var iconBgr = ToGray(icon);

			using Mat result = new();
			Cv2.MatchTemplate(regionBgr, iconBgr, result, TemplateMatchModes.CCoeffNormed);
			Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out _);

			_logger.LogInformation("Icon check [{icon} : {value}]", name, $"{maxVal:F3}");
			return maxVal >= THRESHOLD;
		}

		private static Mat ToGray(Mat mat)
		{
			return mat.Channels() switch
			{
				1 => mat.Clone(),
				3 => mat.CvtColor(ColorConversionCodes.BGR2GRAY),
				4 => mat.CvtColor(ColorConversionCodes.BGRA2GRAY),
				_ => throw new NotSupportedException($"Unsupported channel count: {mat.Channels()}")
			};
		}

		private static Mat CaptureRegion(int x, int y, int width, int height)
		{
			using var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			using Graphics g = Graphics.FromImage(bmp);
			g.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(width, height));
			return BitmapConverter.ToMat(bmp);
		}
	}
}
