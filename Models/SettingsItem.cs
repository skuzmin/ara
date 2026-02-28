using ARA.Interfaces;

namespace ARA.Models
{
	public class SettingsItem : IFilterable
	{
		public string Id { get; set; } = "";
		public string Name { get; set; } = "";
		public string Path { get; set; } = "";
		public override string ToString() => Name;
	}
}
