using System.Windows.Input;
using ARA.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace ARA.ViewModels.Shell
{
	public class MainViewModel : ViewModelBase
	{
		private readonly IServiceProvider _services;
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
			_services = services;
			ShowLoadoutCommand = new RelayCommand(_ => NavigateToPage<LoadoutViewModel>());
			ShowSettingsCommand = new RelayCommand(_ => NavigateToPage<SettingsViewModel>());
			ShowAboutCommand = new RelayCommand(_ => NavigateToPage<AboutViewModel>());
			NavigateToPage<LoadoutViewModel>();
		}

		private void NavigateToPage<TViewModel>() where TViewModel : ViewModelBase
		{
			CurrentPage = _services.GetRequiredService<TViewModel>();
		}
	}
}
