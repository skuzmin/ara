namespace ARA.Models
{
	public class SettingsConfiguration
	{
		public string Locale { get; set; } = "";
		public string Theme { get; set; } = "";
		public string DebugLevel { get; set; } = "";
		public string CaptureMode { get; set; } = "";
		public ScreenCoordinates Coordinates { get; set; } = new ScreenCoordinates();
	}
}
