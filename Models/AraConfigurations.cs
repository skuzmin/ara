namespace ARA.Models
{
	public class AraConfigurations
	{
		public List<LoadoutConfiguration> LoadoutConfigurations { get; set; }
		public SettingsConfiguration SettingsConfiguration { get; set; }
		public AraConfigurations()
		{
			LoadoutConfigurations = [];
			SettingsConfiguration = new();
		}
	}
}
