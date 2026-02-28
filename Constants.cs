using System.Collections.Generic;
using System.IO;
using ARA.Models;

namespace ARA
{
	public static class Constants
	{
		private static readonly string IconPath = "pack://application:,,,/Assets/Icons/";
		private static readonly string _appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		public static string ConfigFilePath => Path.Combine(_appDataPath, "ARA", "configurations.json");
		public static string LogFilePath => Path.Combine(_appDataPath, "ARA", "ara.log");
		public static readonly List<SettingsItem> Languages =
		[
			new() {Id = "en_GB", Name = "English", Path = String.Concat(IconPath, "Flags/Flag-GB.png")},
			new() {Id = "uk_UA", Name = "Ukranian", Path = String.Concat(IconPath, "Flags/Flag-UA.png")}
		];
		public static readonly List<SettingsItem> Themes =
		[
			new() {Id = "dark", Name = "Dark", Path = String.Concat(IconPath, "Mode/Mode-Dark.png")},
			new() {Id = "light", Name = "Light", Path = String.Concat(IconPath, "Mode/Mode-Light.png")}
		];
	}
}
