using System.Windows.Input;
using ARA.ViewModels.Pages;

namespace ARA.ViewModels.Shell
{
	public class MainViewModel : ViewModelBase
	{
		private ViewModelBase? _currentPage;
		public ViewModelBase? CurrentPage
		{
			get => _currentPage;
			set
			{
				_currentPage = value;
				OnPropertyChanged(nameof(CurrentPage));
				OnPropertyChanged(nameof(IsLoadoutSelected));
				OnPropertyChanged(nameof(IsSettingsSelected));
				OnPropertyChanged(nameof(IsAboutSelected));
			}
		}

		public ICommand ShowLoadoutCommand { get; }
		public ICommand ShowSettingsCommand { get; }
		public ICommand ShowAboutCommand { get; }
		public bool IsLoadoutSelected => CurrentPage is LoadoutViewModel;
		public bool IsSettingsSelected => CurrentPage is SettingsViewModel;
		public bool IsAboutSelected => CurrentPage is AboutViewModel;

		public MainViewModel()
		{
			ShowLoadoutCommand = new RelayCommand(_ => CurrentPage = new LoadoutViewModel());
			ShowSettingsCommand = new RelayCommand(_ => CurrentPage = new SettingsViewModel());
			ShowAboutCommand = new RelayCommand(_ => CurrentPage = new AboutViewModel());
			CurrentPage = new LoadoutViewModel();
		}
	}
}
