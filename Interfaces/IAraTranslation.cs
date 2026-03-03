using ARA.Models;

namespace ARA.Interfaces
{
    public interface IAraTranslation
    {
		public SettingsItem GetLocale();
		public void UpdateLocale(SettingsItem theme);
		public List<SettingsItem> GetLocales();
		public string Translate(string key);

		public void SetLocale();
	}
}
