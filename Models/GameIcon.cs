using ARA.Enums;

namespace ARA.Models
{
    public class GameIcon
    {
		public GameItem Item { get; set; }
		public int Id => (int)Item;
		public string Name => Enum.GetName(Item)!.Replace("_", " ");
		public string Path => $"pack://application:,,,/Assets/Items/{Enum.GetName(Item)}.png";

		public static IEnumerable<GameIcon> GetList()
		{
			return Enum.GetValues<GameItem>().Select(x => new GameIcon { Item = x });
		}
		public static GameIcon GetById(int id)
		{
			return new GameIcon { Item = (GameItem)id };
		}
	}
}
