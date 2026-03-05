using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace ARA.Helpers
{
	public class LoadoutCheckerHelper
	{
		private const double THRESHOLD = 0.8;

		public static Dictionary<string, bool> CheckIcons(int x, int y, int width, int height, string[] iconPaths)
		{
			using var region = CaptureRegion(x, y, width, height);

			var results = new Dictionary<string, bool>();

			foreach (var item in iconPaths)
			{
				using var template = LoadTemplate(item);
				results[item] = MatchSingle(region, template);
			}

			return results;
		}

		private static Mat LoadTemplate(string packUri)
		{
			var uri = new Uri(packUri, UriKind.Absolute);
			var streamInfo = Application.GetResourceStream(uri) ?? throw new FileNotFoundException($"Resource not found: {packUri}");

			using var stream = streamInfo.Stream;
			using var memoryStream = new MemoryStream();
			
			stream.CopyTo(memoryStream);
			byte[] bytes = memoryStream.ToArray();

			return Mat.FromImageData(bytes, ImreadModes.Color);
		}

		private static bool MatchSingle(Mat region, Mat template)
		{
			using var regionBgr = EnsureBGR(region);
			using var templateBgr = EnsureBGR(template);

			using Mat result = new();
			Cv2.MatchTemplate(regionBgr, templateBgr, result, TemplateMatchModes.CCoeffNormed);
			Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out _);
			return maxVal >= THRESHOLD;
		}

		private static Mat EnsureBGR(Mat mat)
		{
			return mat.Channels() switch
			{
				4 => mat.CvtColor(ColorConversionCodes.BGRA2BGR),
				1 => mat.CvtColor(ColorConversionCodes.GRAY2BGR),
				3 => mat.Clone(),
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
