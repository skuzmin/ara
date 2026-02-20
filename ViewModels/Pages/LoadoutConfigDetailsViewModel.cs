using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
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
		private readonly IAraConfigurations _configurations;
		private readonly Boolean _isNewLoadout;
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
			_configurations = configurations;

			var loadoutConfig = _configurations.GetCurrentLoadoutConfig();
			_isNewLoadout = loadoutConfig == null;
			LoadoutConfiguration = loadoutConfig ?? new LoadoutConfiguration();
			Title = _isNewLoadout ? "New Configuration" : LoadoutConfiguration.Name;
			ItemsList = new ObservableCollection<GameItem>(GameItem.GetList());

			BackCommand = new RelayCommand(_ => navigation.NavigateToPage(AraPage.LoadoutConfigs));
			SaveLoadoutConfigurationCommand = new RelayCommand(_ => SaveLoadoutConfiguration());
		}

		private void SaveLoadoutConfiguration()
		{
			if (_isNewLoadout)
			{
				_configurations.CreateLoadoutConfig(LoadoutConfiguration);
			}
			else
			{
				_configurations.UpdateLoadoutConfig(LoadoutConfiguration);
			}
			_navigation.NavigateToPage(AraPage.LoadoutConfigs);
		}
	}
}
