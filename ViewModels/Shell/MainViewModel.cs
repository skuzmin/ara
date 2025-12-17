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
				OnPropertyChanged(nameof(IsHomeSelected));
				OnPropertyChanged(nameof(IsSettingsSelected));
			}
		}

		public ICommand ShowHomeCommand { get; }
		public ICommand ShowSettingsCommand { get; }
		public bool IsHomeSelected => CurrentPage is HomeViewModel;
		public bool IsSettingsSelected => CurrentPage is SettingsViewModel;

		public MainViewModel()
		{
			ShowHomeCommand = new RelayCommand(_ => CurrentPage = new HomeViewModel());
			ShowSettingsCommand = new RelayCommand(_ => CurrentPage = new SettingsViewModel());
			CurrentPage = new HomeViewModel();
		}
	}
}
