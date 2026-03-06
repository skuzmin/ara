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
		public static readonly List<SettingsItem> Locales =
		[
			new() {Id = "en_GB", TranslationKey = "Translation.English", Name = "", Path = String.Concat(IconPath, "Flags/Flag-GB.png")},
			new() {Id = "uk_UA", TranslationKey = "Translation.Ukranian", Name = "", Path = String.Concat(IconPath, "Flags/Flag-UA.png")}
		];
		public static readonly List<SettingsItem> Themes =
		[
			new() {Id = "Dark", TranslationKey = "Theme.Dark", Name = "", Path = String.Concat(IconPath, "Mode/Mode-Dark.png")},
			new() {Id = "Light", TranslationKey = "Theme.Light", Name = "", Path = String.Concat(IconPath, "Mode/Mode-Light.png")}
		];
		public static readonly List<SettingsItem> DebugLevels =
		[
			new() {Id = DebugLevel.Default, TranslationKey = "General.Default", Name = "", Path = String.Concat(IconPath, "DebugLevels/Debug-Default.png")},
			new() {Id = DebugLevel.Detailed, TranslationKey = "Debug.Detailed", Name = "", Path = String.Concat(IconPath, "DebugLevels/Debug-Details.png")}
		];
		public static readonly List<SettingsItem> CaptureModes =
		[
			new() {Id = CaptureMode.Default, TranslationKey = "General.Default", Name = "", Path = String.Concat(IconPath, "CaptureModes/Capture-Default.png")},
			new() {Id = CaptureMode.Ignore, TranslationKey = "CaptureMode.Ignore", Name = "", Path = String.Concat(IconPath, "CaptureModes/Capture-Ignore.png")}
		];

		public static class CaptureMode
		{
			public const string Default = "Default";
			public const string Ignore = "Ignore";
		}

		public static class DebugLevel
		{
			public const string Default = "Default";
			public const string Detailed = "Detailed";
		}
	}
}
