using System.Windows;
using ARA.Helpers;
using ARA.Interfaces;
using ARA.Models;
using Microsoft.Extensions.Logging;

namespace ARA.Services
{
	public class TranslationService : IAraTranslation
	{
		private readonly ILogger _logger;
		private readonly SettingsConfiguration _configurations;
		public event Action? TranslationChanged;

		public TranslationService(IAraConfigurations configurationService, ILogger logger)
		{
			_logger = logger;
			_configurations = configurationService.GetSettingsConfiguration();
		}

		public SettingsItem GetLocale()
		{
			return Constants.Locales.FirstOrDefault(l => l.Id == _configurations.Locale)!;
		}

		public List<SettingsItem> GetLocales()
		{
			return Constants.Locales;
		}

		public string Translate(string key)
		{
			return Application.Current.TryFindResource(key) as string ?? key;
		}

		public void SetLocale()
		{
			var uri = new Uri($"Translations/{_configurations.Locale}.xaml", UriKind.Relative);
			DictionaryHelper.UpdateMergedDictionary(uri, "AppTranslationsDictionary");
			_logger.LogInformation("Set locale: {locale}", _configurations.Locale);
			TranslationChanged?.Invoke();
		}

		public void UpdateLocale(string locale)
		{
			_configurations.Locale = locale;
			SetLocale();
		}
	}
}
