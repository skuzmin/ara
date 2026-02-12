using ARA.Models;

namespace ARA.Interfaces
{
	public interface IAraConfigurations
	{
		AraConfigurations Configurations { get; }
		void InitConfig();
		void SaveConfig();
		public void RemoveLoadoutConfigById(Guid Id);
	}
}
