using System.Windows.Input;
using ARA.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;

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

		public MainViewModel(IServiceProvider services)
		{
			ShowLoadoutCommand = new RelayCommand(_ => CurrentPage = services.GetRequiredService<LoadoutViewModel>());
			ShowSettingsCommand = new RelayCommand(_ => CurrentPage = services.GetRequiredService<SettingsViewModel>());
			ShowAboutCommand = new RelayCommand(_ => CurrentPage = services.GetRequiredService<AboutViewModel>());
			CurrentPage = services.GetRequiredService<LoadoutViewModel>();
		}
	}
}
