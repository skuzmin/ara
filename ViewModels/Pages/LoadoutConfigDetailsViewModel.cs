using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ARA.Dialogs;
using ARA.Enums;
using ARA.Interfaces;
using ARA.Models;
using ARA.Views;
using Microsoft.Extensions.Logging;

namespace ARA.ViewModels.Pages
{
	public class LoadoutConfigDetailsViewModel : ViewModelBase
	{
		private readonly IMainWindow _windowService;
		private readonly ILogger _logger;
		private readonly IAraNavigation _navigation;
		private readonly IAraConfigurations _configurations;
		private readonly Boolean _isNewLoadout;
		public ICommand BackCommand { get; }
		public ICommand AddItemCommand { get; }
		public ICommand RemoveItemCommand { get; }
		public ICommand SaveLoadoutConfigurationCommand { get; }
		public ICommand SelectRegionCommand { get; }
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
				TextBoxValidate(LoadoutValidationField.Name);
			}
		}
		public double? CoordinatesX
		{
			get => field;
			set
			{
				field = value;
				IsEdited = true;
				OnPropertyChanged(nameof(CoordinatesX));
				TextBoxValidate(LoadoutValidationField.CoordinatesX);
			}
		}
		public double? CoordinatesY
		{
			get => field;
			set
			{
				field = value;
				IsEdited = true;
				OnPropertyChanged(nameof(CoordinatesY));
				TextBoxValidate(LoadoutValidationField.CoordinatesY);
			}
		}
		public double? CoordinatesHeight
		{
			get => field;
			set
			{
				field = value;
				IsEdited = true;
				OnPropertyChanged(nameof(CoordinatesHeight));
				TextBoxValidate(LoadoutValidationField.CoordinatesHeight);
			}
		}
		public double? CoordinatesWidth
		{
			get => field;
			set
			{
				field = value;
				IsEdited = true;
				OnPropertyChanged(nameof(CoordinatesWidth));
				TextBoxValidate(LoadoutValidationField.CoordinatesWidth);
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
		public LoadoutConfigDetailsViewModel(IAraNavigation navigation, IAraConfigurations configurations, ILogger logger, IMainWindow windowService)
		{
			// Services(DI)
			_logger = logger;
			_navigation = navigation;
			_configurations = configurations;
			_windowService = windowService;
			// Basic data
			var loadoutConfig = _configurations.GetCurrentLoadoutConfig();
			_isNewLoadout = loadoutConfig == null;
			LoadoutConfiguration = loadoutConfig ?? new LoadoutConfiguration();
			Title = _isNewLoadout ? "New Configuration" : LoadoutConfiguration.Name;
			// Init lists for Combobox/DataGrid
			var allGameItems = GameItem.GetList();
			var filteredList = allGameItems.Where(a => !LoadoutConfiguration.Items.Any(b => b.Id == a.Id)).ToList();
			LoadoutValidation = new LoadoutConfigurationValidation();
			Name = LoadoutConfiguration.Name;
			CoordinatesX = LoadoutConfiguration.Coordinates.X;
			CoordinatesY = LoadoutConfiguration.Coordinates.Y;
			CoordinatesHeight = LoadoutConfiguration.Coordinates.Height;
			CoordinatesWidth = LoadoutConfiguration.Coordinates.Width;
			ItemsList = new ObservableCollection<GameItem>(filteredList);
			SelectedItemsList = new ObservableCollection<GameItem>(LoadoutConfiguration.Items);
			IsEdited = false;
			// Commands
			AddItemCommand = new RelayCommand(_ => AddItem());
			RemoveItemCommand = new RelayCommand(item => RemoveItem((GameItem)item));
			BackCommand = new RelayCommand(_ => navigation.TryNavigateToPage(AraPage.LoadoutConfigs));
			SaveLoadoutConfigurationCommand = new RelayCommand(_ => SaveLoadoutConfiguration());
			SelectRegionCommand = new RelayCommand(_ => SelectRegion());
		}

		private void TextBoxValidate(LoadoutValidationField field)
		{
			if (!LoadoutValidation.IsValidated)
			{
				return;
			}

			switch (field)
			{
				case LoadoutValidationField.Name:
					LoadoutValidation.IsNameNotValid = Name == null || string.IsNullOrEmpty(Name);
					break;
				case LoadoutValidationField.CoordinatesX:
					LoadoutValidation.IsCoordinateXNotValid = CoordinatesX == null;
					break;
				case LoadoutValidationField.CoordinatesY:
					LoadoutValidation.IsCoordinateYNotValid = CoordinatesY == null;
					break;
				case LoadoutValidationField.CoordinatesHeight:
					LoadoutValidation.IsCoordinateHeightNotValid = CoordinatesHeight == null;
					break;
				case LoadoutValidationField.CoordinatesWidth:
					LoadoutValidation.IsCoordinateWidthNotValid = CoordinatesWidth == null;
					break;
			}
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
				Title = "Unsaved changes",
				Message = $"All unsaved changes will be lost.",
				SubMessage = "Are you sure you want to discard changes?",
				ConfirmButtonText = "Discard",
				CancelButtonText = "Cancel"
			};

			var result = new ConfirmationDialog(dialogConfig)
			{
				Owner = Application.Current.MainWindow
			}.ShowDialog();

			return result ?? false;
		}

		private void SelectRegion()
		{
			_windowService.HideMainWindow();
			var coordinates = new ScreenCoordinates(CoordinatesX, CoordinatesY, CoordinatesHeight, CoordinatesWidth);
			var overlay = new OverlayWindow(coordinates);
			overlay.OnSave += coordinates =>
			{
				CoordinatesX = coordinates.X;
				CoordinatesY = coordinates.Y;
				CoordinatesHeight = coordinates.Height;
				CoordinatesWidth = coordinates.Width;
			};
			overlay.Closed += (s, args) => _windowService.ShowMainWindow();
			overlay.Show();
		}

		private void SaveLoadoutConfiguration()
		{
			LoadoutValidation.IsValidated = true;
			foreach (var value in Enum.GetValues<LoadoutValidationField>())
			{
				TextBoxValidate(value);
			}
			ListValidate();

			if (!LoadoutValidation.IsValid)
			{
				return;
			}
			IsEdited = false;
			LoadoutConfiguration.Items = [.. SelectedItemsList];
			LoadoutConfiguration.Name = Name;
			LoadoutConfiguration.Coordinates = new ScreenCoordinates(CoordinatesX, CoordinatesY, CoordinatesHeight, CoordinatesWidth);
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
