using ARA.Models;

namespace ARA.Interfaces
{
    public interface IAraTranslation
    {
		public event Action? TranslationChanged;
		public SettingsItem GetLocale();
		public void UpdateLocale(string locale);
		public List<SettingsItem> GetLocales();
		public string Translate(string key);
		public void SetLocale();
	}
}
