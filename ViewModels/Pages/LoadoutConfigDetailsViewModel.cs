using System.Collections.ObjectModel;
using System.Windows.Input;
using ARA.Dialogs;
using ARA.Enums;
using ARA.Interfaces;
using ARA.Models;
using Microsoft.Extensions.Logging;

namespace ARA.ViewModels.Pages
{
	public class LoadoutConfigDetailsViewModel : ViewModelBase
	{
		private readonly IAraTranslation _translations;
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
		public Action? ResetComboBox { get; set; }
		public LoadoutConfigurationValidation LoadoutValidation { get; set; }
		public bool IsEdited
		{
			get => field;
			set
			{
				field = value;
				OnPropertyChanged(nameof(IsEdited));
			}
		}
		public string Name
		{
			get => field;
			set
			{
				field = value;
				IsEdited = true;
				OnPropertyChanged(nameof(Name));
				TextBoxValidate();
			}
		}
		public GameItem? SelectedItem
		{
			get => field;
			set
			{
				field = value;
				OnPropertyChanged(nameof(SelectedItem));
			}
		}
		public bool IsValid => LoadoutValidation.IsValid;
		public LoadoutConfigDetailsViewModel(
			IAraNavigation navigation, 
			IAraConfigurations configurations, 
			ILogger logger,
			IAraTranslation translation)
		{
			// Services(DI)
			_translations = translation;
			_logger = logger;
			_navigation = navigation;
			_configurations = configurations;
			// Basic data
			var loadoutConfig = _configurations.GetCurrentLoadoutConfig();
			_isNewLoadout = loadoutConfig == null;
			LoadoutConfiguration = loadoutConfig ?? new LoadoutConfiguration();
			Title = _isNewLoadout ? _translations.Translate("LoadoutConfig.NewConfigration") : LoadoutConfiguration.Name;
			// Init lists for Combobox/DataGrid
			var allGameItems = GameItem.GetList();
			var filteredList = allGameItems.Where(a => !LoadoutConfiguration.Items.Any(b => b.Id == a.Id)).ToList();
			LoadoutValidation = new LoadoutConfigurationValidation();
			Name = LoadoutConfiguration.Name;
			ItemsList = new ObservableCollection<GameItem>(filteredList);
			SelectedItemsList = new ObservableCollection<GameItem>(LoadoutConfiguration.Items);
			IsEdited = false;
			// Commands
			AddItemCommand = new RelayCommand(_ => AddItem());
			RemoveItemCommand = new RelayCommand(item => RemoveItem((GameItem)item));
			BackCommand = new RelayCommand(_ => navigation.TryNavigateToPage(AraPage.LoadoutConfigs));
			SaveLoadoutConfigurationCommand = new RelayCommand(_ => SaveLoadoutConfiguration());
		}

		private void TextBoxValidate()
		{
			if (!LoadoutValidation.IsValidated)
			{
				return;
			}
			LoadoutValidation.IsNameNotValid = Name == null || string.IsNullOrEmpty(Name);

			OnPropertyChanged(nameof(LoadoutValidation));
			OnPropertyChanged(nameof(IsValid));
		}

		private void ListValidate()
		{
			if (!LoadoutValidation.IsValidated)
			{
				return;
			}

			LoadoutValidation.IsItemsListNotValid = SelectedItemsList.Count == 0;
			OnPropertyChanged(nameof(LoadoutValidation));
			OnPropertyChanged(nameof(IsValid));
		}

		public override bool CanNavigateAway()
		{
			if (!IsEdited)
			{
				return true;
			}

			var dialogConfig = new ConfirmationDialogConfig
			{
				Title = _translations.Translate("General.Unsaved.Title"),
				Message = _translations.Translate("General.Unsaved.Message"),
				SubMessage = _translations.Translate("General.Unsaved.SubMessage"),
				ConfirmButtonText = _translations.Translate("General.Unsaved.ConfirmButton"),
				CancelButtonText = _translations.Translate("General.Unsaved.CancelButton")
			};

			var result = new ConfirmationDialog(dialogConfig).ShowDialog();

			return result ?? false;
		}

		private void SaveLoadoutConfiguration()
		{
			LoadoutValidation.IsValidated = true;
			TextBoxValidate();
			ListValidate();

			if (!LoadoutValidation.IsValid)
			{
				return;
			}
			IsEdited = false;
			LoadoutConfiguration.Items = [.. SelectedItemsList];
			LoadoutConfiguration.Name = Name;
			if (_isNewLoadout)
			{
				_configurations.CreateLoadoutConfig(LoadoutConfiguration);
			}
			else
			{
				_configurations.UpdateLoadoutConfig(LoadoutConfiguration);
			}
			_navigation.TryNavigateToPage(AraPage.LoadoutConfigs);
		}

		private void AddItem()
		{
			if (SelectedItem is not { } item)
			{
				return;
			}
			IsEdited = true;
			SelectedItemsList.Add(item);
			ItemsList.Remove(item);
			ResetComboBox?.Invoke();
			ListValidate();
			_logger.LogInformation("Add {item} to {loadout}", item.Name, LoadoutConfiguration.Name);
		}

		private void RemoveItem(GameItem item)
		{
			IsEdited = true;
			SelectedItemsList.Remove(item);
			ItemsList = new ObservableCollection<GameItem>(
				ItemsList
					.Append(item)
					.OrderBy(x => x.Name)
			);
			OnPropertyChanged(nameof(ItemsList));
			ListValidate();
			_logger.LogInformation("Remove {item} from {loadout}", item.Name, LoadoutConfiguration.Name);
		}
	}
}
