
namespace ARA.Models
{
    public class LoadoutConfiguration
    {
        public Guid Id { get; } = Guid.NewGuid();
		public string Name { get; set; } = "New Loadout";
        public ScreenCoordinates Coordinates { get; set; } = new ScreenCoordinates();
        public List<GameItem> Items { get; set; } = [];
    }
}
