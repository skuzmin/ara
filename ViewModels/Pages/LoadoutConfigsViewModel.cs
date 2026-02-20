using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using ARA.Dialogs;
using ARA.Enums;
using ARA.Interfaces;
using ARA.Models;

namespace ARA.ViewModels.Pages
{
	public class LoadoutConfigsViewModel : ViewModelBase
	{
		private readonly IAraConfigurations _configurations;
		private readonly IAraNavigation _navigation;
		public ObservableCollection<LoadoutConfiguration> LoadoutConfigurations { get; set; }
		public ICommand BackCommand { get; }
		public ICommand NewConfigCommand { get; }
		public ICommand EditConfigCommand { get; }
		public ICommand DeleteConfigCommand { get; }

		public LoadoutConfigsViewModel(IAraNavigation navigation, IAraConfigurations configurations)
		{
			_configurations = configurations;
			_navigation = navigation;
			LoadoutConfigurations = new ObservableCollection<LoadoutConfiguration>(_configurations.Configurations.LoadoutConfigurations);
			BackCommand = new RelayCommand(_ => navigation.NavigateToPage(AraPage.Loadout));
			NewConfigCommand = new RelayCommand(_ => NewConfiguration());
			EditConfigCommand = new RelayCommand(item => EditConfiguration((LoadoutConfiguration)item));
			DeleteConfigCommand = new RelayCommand(data => DeleteConfiguration((LoadoutConfiguration)data));
		}

		private void NewConfiguration()
		{
			_configurations.SetCurrentLoadoutConfig(null);
			_navigation.NavigateToPage(AraPage.LoadoutConfigDetails);
		}

		private void EditConfiguration(LoadoutConfiguration loadout)
		{
			_configurations.SetCurrentLoadoutConfig(loadout);
			_navigation.NavigateToPage(AraPage.LoadoutConfigDetails);
		}

		private void DeleteConfiguration(LoadoutConfiguration data)
		{
			var dialogConfig = new ConfirmationDialogConfig
			{
				Title = "Delete Configuration",
				Message = $"Are you sure you want to delete configuration ?",
				SubMessage = $"[{data.Name}]",
				ConfirmButtonText = "Delete",
				CancelButtonText = "Cancel"
			};
			var dialog = new ConfirmationDialog(dialogConfig)
			{
				Owner = Application.Current.MainWindow
			};
			var result = dialog.ShowDialog();
			if (result == true)
			{
				_configurations.RemoveLoadoutConfigById(data.Id);
				LoadoutConfigurations = new ObservableCollection<LoadoutConfiguration>(_configurations.Configurations.LoadoutConfigurations);
				OnPropertyChanged(nameof(LoadoutConfigurations));
			}
		}
	}
}
