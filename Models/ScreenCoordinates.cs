namespace ARA.Models
{
	public class ScreenCoordinates
	{
		private const double DefaultCoordinate = 100;
		private const double DefaultSize = 300;
		public double X { get; set; } = DefaultCoordinate;
		public double Y { get; set; } = DefaultCoordinate;
		public double Height { get; set; } = DefaultSize;
		public double Width { get; set; } = DefaultSize;

		public ScreenCoordinates() { }
		public ScreenCoordinates(double? x, double? y, double? h, double? w)
		{
			X = x ?? DefaultCoordinate;
			Y = y ?? DefaultCoordinate;
			Height = h ?? DefaultSize;
			Width = w ?? DefaultSize;
		}
	}
}