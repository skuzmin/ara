using ARA.Models;

namespace ARA.Interfaces
{
	public interface ILoadoutCheckerService
	{
		public Dictionary<int, bool> CheckIcons(List<GameItem> icons);
		public void InitGameWindow();
		public bool IsGameDetected();

	}
}
