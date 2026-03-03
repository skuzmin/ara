using System.Windows;
using ARA.Helpers;
using ARA.Interfaces;
using ARA.Models;

namespace ARA.Services
{
	public class TranslationService : IAraTranslation
	{
		private readonly IAraConfigurations _configurationService;
		private readonly SettingsConfiguration _configurations;
		private SettingsItem _locale;
		public TranslationService(IAraConfigurations configurationService)
		{
			_configurationService = configurationService;
			_configurations = configurationService.GetSettingsConfiguration();
			_locale = Constants.Locales.FirstOrDefault(l => l.Id == _configurations.Locale) ?? Constants.Locales[0];
		}

		public SettingsItem GetLocale()
		{
			return _locale;
		}

		public List<SettingsItem> GetLocales()
		{
			return Constants.Locales;
		}

		public string Translate(string key)
		{
			throw new NotImplementedException();
		}

		public void SetLocale()
		{
			var uri = new Uri($"Translations/{_locale.Id}.xaml", UriKind.Relative);
			DictionaryHelper.UpdateMergedDictionary(uri, "AppTranslationsDictionary");
		}

		public void UpdateLocale(SettingsItem locale)
		{
			_locale = locale;
			_configurations.Locale = locale.Id;
			_configurationService.UpdateSettings(_configurations);
			SetLocale();
		}
	}
}
