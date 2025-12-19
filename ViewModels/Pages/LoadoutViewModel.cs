using System.Windows.Input;

namespace ARA.ViewModels.Pages
{
	public class LoadoutViewModel : ViewModelBase
	{
		private string _title = "Loadout page";
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged(nameof(Title));
			}
		}

		public ICommand ChangeTitleCommand { get; }

		public LoadoutViewModel()
		{
			ChangeTitleCommand = new RelayCommand(_ => Title = "Loadout title changed");
		}
	}
}
