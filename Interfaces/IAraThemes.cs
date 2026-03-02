using ARA.Models;

namespace ARA.Interfaces
{
	public interface IAraThemes
	{
		public SettingsItem GetTheme();
		public void UpdateTheme(SettingsItem theme);
		public List<SettingsItem> GetThemes();
		public void ActivateTheme();
	}
}
