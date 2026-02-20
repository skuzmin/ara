using System.Collections.ObjectModel;
using System.Windows.Input;
using ARA.Enums;
using ARA.Interfaces;
using ARA.Models;
using Microsoft.Extensions.Logging;

namespace ARA.ViewModels.Pages
{
	public class LoadoutConfigDetailsViewModel : ViewModelBase
	{
		private readonly ILogger _logger;
		private readonly IAraNavigation _navigation;
		public ICommand BackCommand { get; }
		public ICommand SaveLoadoutConfigurationCommand { get; }
		public ObservableCollection<GameItem> ItemsList { get; }
		public LoadoutConfiguration LoadoutConfiguration { get; }
		public string Title { get; }
		public GameItem? SelectedItem
		{
			get => field;
			set
			{
				if (value != null)
				{
					_logger.LogInformation("Change loadout to: {loadout}", value.Name);
				}
				field = value;
				OnPropertyChanged(nameof(SelectedItem));
			}
		}

		public LoadoutConfigDetailsViewModel(IAraNavigation navigation, IAraConfigurations configurations, ILogger logger)
		{
			_logger = logger;
			_navigation = navigation;
			LoadoutConfiguration = configurations.GetCurrentLoadoutConfig();
			Title = string.IsNullOrEmpty(LoadoutConfiguration.Name) ? "New Configuration" : LoadoutConfiguration.Name;
			BackCommand = new RelayCommand(_ => navigation.NavigateToPage(AraPage.LoadoutConfigs));
			SaveLoadoutConfigurationCommand = new RelayCommand(_ => SaveLoadoutConfiguration());
			ItemsList = new ObservableCollection<GameItem>(GameItem.GetList());
		}

		private void SaveLoadoutConfiguration()
		{
			//_navigation.NavigateToPage(AraPage.LoadoutConfigs);
		}
	}
}
