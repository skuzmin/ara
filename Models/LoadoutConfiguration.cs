
namespace ARA.Models
{
    public class LoadoutConfiguration
    {
        public Guid Id { get; } = Guid.NewGuid();
		public string Name { get; set; } = "";
        public List<GameItem> Items { get; set; } = [];
    }
}
