namespace ARA.Models
{
	public class ScreenCoordinates
	{
		public double X { get; set; } = 100;
		public double Y { get; set; } = 100;
		public double Height { get; set; } = 300;
		public double Width { get; set; } = 300;

		public ScreenCoordinates Clone()
		{
			return new ScreenCoordinates
			{
				X = X,
				Y = Y,
				Height = Height,
				Width = Width
			};
		}
	}
}
