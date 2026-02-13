using System.Collections.ObjectModel;
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
		public ObservableCollection<LoadoutConfiguration> LoadoutConfigurations { get; set; }
		public ICommand BackCommand { get; }
		public ICommand ConfigDetailsCommand { get; }
		public ICommand DeleteConfigCommand { get; }

		public LoadoutConfigsViewModel(IAraNavigation navigations, IAraConfigurations configurations)
		{
			_configurations = configurations;
			LoadoutConfigurations = new ObservableCollection<LoadoutConfiguration>(_configurations.Configurations.LoadoutConfigurations);
			BackCommand = new RelayCommand(_ => navigations.NavigateToPage(AraPage.Loadout));
			ConfigDetailsCommand = new RelayCommand(_ => navigations.NavigateToPage(AraPage.LoadoutConfigDetails));
			DeleteConfigCommand = new RelayCommand(data => DeleteConfiguration((LoadoutConfiguration)data));
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
