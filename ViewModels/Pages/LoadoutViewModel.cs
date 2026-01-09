using System.Collections.ObjectModel;
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

		public ObservableCollection<string> LoadoutOptions { get; }

		private string _selectedLoadout;
		public string SelectedLoadout
		{
			get => _selectedLoadout;
			set
			{
				_selectedLoadout = value;
				OnPropertyChanged(nameof(SelectedLoadout));
			}
		}

		public LoadoutViewModel()
		{
			_selectedLoadout = "Option 2";
			LoadoutOptions = ["Option 1", "Option 2", "Option 3", "Option 4"];
			ChangeTitleCommand = new RelayCommand(_ => Title = "Loadout title changed");
		}
	}
}
