using System.Windows;
using ARA.Interfaces;
using ARA.Models;

namespace ARA.Services
{
	public class ThemesService : IAraThemes
	{
		private readonly IAraConfigurations _configurationService;
		private readonly SettingsConfiguration _configurations;
		private SettingsItem _theme;
		public ThemesService(IAraConfigurations configurationService)
		{
			_configurationService = configurationService;
			_configurations = configurationService.GetSettingsConfiguration();
			_theme = Constants.Themes.FirstOrDefault(l => l.Id == _configurations.Theme) ?? Constants.Themes[0];
		}

		public SettingsItem GetTheme()
		{
			return _theme;
		}

		public List<SettingsItem> GetThemes()
		{
			return Constants.Themes;
		}

		public void ActivateTheme()
		{
			var uri = new Uri($"Themes/{_theme.Name}.xaml", UriKind.Relative);
			var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
			var dictionary = mergedDictionaries.FirstOrDefault(d =>
				d.Contains("Title") && d["Title"] as string == "AppThemeDictionary");

			if (dictionary != null)
			{
				mergedDictionaries.Remove(dictionary);
			}

			mergedDictionaries.Add(new ResourceDictionary { Source = uri });
		}

		public void UpdateTheme(SettingsItem theme)
		{
			_theme = theme;
			_configurations.Theme = theme.Id;
			ActivateTheme();
			_configurationService.UpdateSettings(_configurations);
		}
	}
}
