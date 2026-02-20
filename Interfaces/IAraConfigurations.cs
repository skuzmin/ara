using ARA.Models;

namespace ARA.Interfaces
{
	public interface IAraConfigurations
	{
		AraConfigurations Configurations { get; }
		void InitConfig();
		void SaveConfig();
		public LoadoutConfiguration? GetCurrentLoadoutConfig();
		public void SetCurrentLoadoutConfig(LoadoutConfiguration? loadout);
		public void CreateLoadoutConfig(LoadoutConfiguration loadout);
		public void UpdateLoadoutConfig(LoadoutConfiguration loadout);
		public void RemoveLoadoutConfigById(Guid Id);
	}
}
