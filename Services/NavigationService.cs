using System.ComponentModel;
using System.Windows.Input;
using ARA.Enums;
using ARA.Interfaces;
using ARA.ViewModels;
using ARA.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ARA.Services
{
	internal class NavigationService : IAraNavigation
	{
		private readonly IServiceProvider _services;
		private readonly ILogger _logger;
		public ICommand NavigateToPageCommand { get; }
		public event PropertyChangedEventHandler? PropertyChanged;
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
				CommandManager.InvalidateRequerySuggested();
			}
		}
		public bool IsLoadoutSelected => CurrentPage is LoadoutViewModel || CurrentPage is LoadoutConfigsViewModel;
		public bool IsSettingsSelected => CurrentPage is SettingsViewModel;
		public bool IsAboutSelected => CurrentPage is AboutViewModel;

		public NavigationService(IServiceProvider services, ILogger logger)
		{
			_logger = logger;
			_services = services;
			NavigateToPageCommand = new RelayCommand(
				page => NavigateToPage((AraPage)page),
				canExecute: page => CanNavigateToPage((AraPage)page));
		}

		public void NavigateToDefaultPage()
		{
			GoToPage<LoadoutViewModel>();
		}

		public void NavigateToPage(AraPage page)
		{
			switch (page)
			{
				case AraPage.Loadout:
					GoToPage<LoadoutViewModel>();
					break;
				case AraPage.LoadoutConfigs:
					GoToPage<LoadoutConfigsViewModel>();
					break;
				case AraPage.Settings:
					GoToPage<SettingsViewModel>();
					break;
				case AraPage.About:
					GoToPage<AboutViewModel>();
					break;
				default:
					NavigateToDefaultPage();
					break;
			}
		}

		private bool CanNavigateToPage(AraPage page)
		{
			return page switch
			{
				AraPage.Loadout => !IsLoadoutSelected,
				AraPage.Settings => !IsSettingsSelected,
				AraPage.About => !IsAboutSelected,
				_ => true
			};
		}


		private void GoToPage<TViewModel>() where TViewModel : ViewModelBase
		{
#pragma warning disable CA1873
			_logger.LogInformation("Naviage to page: {page}", typeof(TViewModel).Name);
#pragma warning restore CA1873
			CurrentPage = _services.GetRequiredService<TViewModel>();
		}

		protected void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
