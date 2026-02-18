using ARA.Enums;
using ARA.Interfaces;

namespace ARA.Models
{
	public class GameItem : IFilterable
	{
		public GameIcon Icon { get; set; }
		public int Quantity { get; set; } = 1;
		public GameItemStatus Status { get; set; } = GameItemStatus.Unknown;
		public int Id => (int)Icon;
		public string Name => Enum.GetName(Icon)!.Replace("_", " ");
		public string Path => $"pack://application:,,,/Assets/Items/{Enum.GetName(Icon)}.png";

		public static IEnumerable<GameItem> GetList()
		{
			return Enum.GetValues<GameIcon>().Select(x => new GameItem { Icon = x });
		}
	}
}
