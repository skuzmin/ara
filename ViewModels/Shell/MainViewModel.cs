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
			}
		}

		public ICommand ShowHomeCommand { get; }
		public ICommand ShowSettingsCommand { get; }

		public MainViewModel()
		{
			ShowHomeCommand = new RelayCommand(_ => CurrentPage = new HomeViewModel());
			ShowSettingsCommand = new RelayCommand(_ => CurrentPage = new SettingsViewModel());
			CurrentPage = new HomeViewModel();
		}
	}
}
