
namespace ARA.Models
{
    public class LoadoutConfiguration
    {
        public readonly Guid Id = Guid.NewGuid();
		public string Name { get; set; } = "New Loadout";
        public ScreenCoordinates Coordinates { get; set; } = new ScreenCoordinates();
        public List<GameItem> Items { get; set; } = [];
    }
}
