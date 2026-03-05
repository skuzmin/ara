using ARA.Helpers;
using ARA.Interfaces;
using ARA.Models;

namespace ARA.Services
{
	public class ThemesService : IAraThemes
	{
		private readonly SettingsConfiguration _configurations;
		public event Action? ThemeChanged;
		public ThemesService(IAraConfigurations configurationService)
		{
			_configurations = configurationService.GetSettingsConfiguration();
		}

		public SettingsItem GetTheme()
		{
			return Constants.Themes.FirstOrDefault(t => t.Id == _configurations.Theme)!;
		}

		public List<SettingsItem> GetThemes()
		{
			return Constants.Themes;
		}

		public void ActivateTheme()
		{
			var uri = new Uri($"Themes/{_configurations.Theme}.xaml", UriKind.Relative);
			DictionaryHelper.UpdateMergedDictionary(uri, "AppThemeDictionary");
			ThemeChanged?.Invoke();
		}

		public void UpdateTheme(string theme)
		{
			_configurations.Theme = theme;
			ActivateTheme();
		}
	}
}
