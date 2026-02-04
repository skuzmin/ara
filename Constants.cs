using System.IO;

namespace ARA
{
	public static class Constants
	{
		private static readonly string _appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		public static string ConfigFilePath => Path.Combine(_appDataPath, "ARA", "configurations.json");
		public static string LogFilePath => Path.Combine(_appDataPath, "ARA", "ara.log");
	}
}
