using ARA.Helpers;
using ARA.Interfaces;
using ARA.Models;

namespace ARA.Services
{
	public class ThemesService : IAraThemes
	{
		private readonly IAraConfigurations _configurationService;
		private readonly SettingsConfiguration _configurations;
		private SettingsItem _theme;
		public event Action? ThemeChanged;
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
			var uri = new Uri($"Themes/{_theme.Id}.xaml", UriKind.Relative);
			DictionaryHelper.UpdateMergedDictionary(uri, "AppThemeDictionary");
			ThemeChanged?.Invoke();
		}

		public void UpdateTheme(SettingsItem theme)
		{
			_theme = theme;
			_configurations.Theme = theme.Id;
			_configurationService.UpdateSettings(_configurations);
			ActivateTheme();
		}
	}
}
