using ARA.Enums;

namespace ARA.ViewModels.Pages
{
	public class SettingsViewModel : ViewModelBase
	{
		private string _title = "Settings page";
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged(nameof(Title));
			}
		}
	}
}
