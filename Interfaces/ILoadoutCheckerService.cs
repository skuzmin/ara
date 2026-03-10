using ARA.Models;

namespace ARA.Interfaces
{
	public interface ILoadoutCheckerService
	{
		public Dictionary<int, bool> CheckIcons(int x, int y, int width, int height, List<GameItem> icons);
		public void InitGameWindow();
		public bool IsGameDetected();

	}
}
