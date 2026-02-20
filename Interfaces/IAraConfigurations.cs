using ARA.Models;

namespace ARA.Interfaces
{
	public interface IAraConfigurations
	{
		AraConfigurations Configurations { get; }
		void InitConfig();
		void SaveConfig();
		public LoadoutConfiguration GetCurrentLoadoutConfig();
		public void SetCurrentConfigurationById(Guid id);
		public void SetCurrentConfigurationAsNew();
		public void RemoveLoadoutConfigById(Guid Id);
	}
}
