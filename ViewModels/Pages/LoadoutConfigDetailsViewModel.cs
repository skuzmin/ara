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
		private readonly IAraConfigurations _configurations;
		private readonly Boolean _isNewLoadout;
		public ICommand BackCommand { get; }
		public ICommand AddItemCommand { get; }
		public ICommand RemoveItemCommand { get; }
		public ICommand SaveLoadoutConfigurationCommand { get; }
		public ObservableCollection<GameItem> ItemsList { get; set; }
		public ObservableCollection<GameItem> SelectedItemsList { get; }
		public LoadoutConfiguration LoadoutConfiguration { get; }
		public string Title { get; }
		public GameItem? SelectedItem
		{
			get => field;
			set
			{
				field = value;
				OnPropertyChanged(nameof(SelectedItem));
			}
		}
		public LoadoutConfigDetailsViewModel(IAraNavigation navigation, IAraConfigurations configurations, ILogger logger)
		{
			// Services(DI)
			_logger = logger;
			_navigation = navigation;
			_configurations = configurations;
			// Basic data
			var loadoutConfig = _configurations.GetCurrentLoadoutConfig();
			_isNewLoadout = loadoutConfig == null;
			LoadoutConfiguration = loadoutConfig ?? new LoadoutConfiguration();
			Title = _isNewLoadout ? "New Configuration" : LoadoutConfiguration.Name;
			// Init lists for Combobox/DataGrid
			var allGameItems = GameItem.GetList();
			var filteredList = allGameItems.Where(a => !LoadoutConfiguration.Items.Any(b => b.Id == a.Id)).ToList();
			ItemsList = new ObservableCollection<GameItem>(filteredList);
			SelectedItemsList = new ObservableCollection<GameItem>(LoadoutConfiguration.Items);
			// Commands
			AddItemCommand = new RelayCommand(_ => AddItem());
			RemoveItemCommand = new RelayCommand(item => RemoveItem((GameItem)item));
			BackCommand = new RelayCommand(_ => navigation.NavigateToPage(AraPage.LoadoutConfigs));
			SaveLoadoutConfigurationCommand = new RelayCommand(_ => SaveLoadoutConfiguration());
		}

		private void SaveLoadoutConfiguration()
		{
			LoadoutConfiguration.Items = [.. SelectedItemsList];
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

		private void AddItem()
		{
			if (SelectedItem is not { } item)
			{
				return;
			}

			SelectedItemsList.Add(item);
			ItemsList.Remove(item);
			SelectedItem = null;
			_logger.LogInformation("Add {item} to {loadout}", item.Name, LoadoutConfiguration.Name);
		}

		private void RemoveItem(GameItem item)
		{
			SelectedItemsList.Remove(item);
			ItemsList = new ObservableCollection<GameItem>(
				ItemsList
					.Append(item)
					.OrderBy(x => x.Name)
			);
			OnPropertyChanged(nameof(ItemsList));
			_logger.LogInformation("Remove {item} from {loadout}", item.Name, LoadoutConfiguration.Name);
		}
	}
}
