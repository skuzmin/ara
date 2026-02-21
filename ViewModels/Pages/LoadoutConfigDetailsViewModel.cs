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
			_logger = logger;
			_navigation = navigation;
			_configurations = configurations;

			var loadoutConfig = _configurations.GetCurrentLoadoutConfig();
			_isNewLoadout = loadoutConfig == null;
			LoadoutConfiguration = loadoutConfig ?? new LoadoutConfiguration();
			Title = _isNewLoadout ? "New Configuration" : LoadoutConfiguration.Name;

			var allGameItems = GameItem.GetList();
			var filteredList = allGameItems.Where(a => !LoadoutConfiguration.Items.Any(b => b.Id == a.Id)).ToList();
			ItemsList = new ObservableCollection<GameItem>(filteredList);
			SelectedItemsList = new ObservableCollection<GameItem>(LoadoutConfiguration.Items);
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
			SelectedItemsList.Add(SelectedItem!);
			ItemsList.Remove(SelectedItem!);
			SelectedItem = null;
		}

		private void RemoveItem(GameItem item)
		{
			SelectedItemsList.Remove(item);
			ItemsList.Add(item);
			ItemsList = new ObservableCollection<GameItem>(ItemsList.OrderBy(x => x.Name));
		}
	}
}
