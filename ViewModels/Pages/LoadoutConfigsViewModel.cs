using System.Collections.ObjectModel;
using System.Windows.Input;
using ARA.Dialogs;
using ARA.Enums;
using ARA.Interfaces;
using ARA.Models;

namespace ARA.ViewModels.Pages
{
	public class LoadoutConfigsViewModel : ViewModelBase
	{
		private readonly IAraTranslation _translations;
		private readonly IAraConfigurations _configurations;
		private readonly IAraNavigation _navigation;
		public ObservableCollection<LoadoutConfiguration> LoadoutConfigurations { get; set; }
		public ICommand BackCommand { get; }
		public ICommand NewConfigCommand { get; }
		public ICommand EditConfigCommand { get; }
		public ICommand DeleteConfigCommand { get; }

		public LoadoutConfigsViewModel(IAraNavigation navigation, IAraConfigurations configurations, IAraTranslation translation)
		{
			_translations = translation;
			_configurations = configurations;
			_navigation = navigation;
			LoadoutConfigurations = new ObservableCollection<LoadoutConfiguration>(_configurations.Configurations.LoadoutConfigurations);
			BackCommand = new RelayCommand(_ => navigation.TryNavigateToPage(AraPage.Loadout));
			NewConfigCommand = new RelayCommand(_ => NewConfiguration());
			EditConfigCommand = new RelayCommand(item => EditConfiguration((LoadoutConfiguration)item));
			DeleteConfigCommand = new RelayCommand(data => DeleteConfiguration((LoadoutConfiguration)data));
		}

		private void NewConfiguration()
		{
			_configurations.SetCurrentLoadoutConfig(null);
			_navigation.TryNavigateToPage(AraPage.LoadoutConfigDetails);
		}

		private void EditConfiguration(LoadoutConfiguration loadout)
		{
			_configurations.SetCurrentLoadoutConfig(loadout);
			_navigation.TryNavigateToPage(AraPage.LoadoutConfigDetails);
		}

		private void DeleteConfiguration(LoadoutConfiguration data)
		{
			var dialogConfig = new ConfirmationDialogConfig
			{
				Title = _translations.Translate("General.Confirmation.Title"),
				Message = _translations.Translate("General.Confirmation.Message"),
				SubMessage = $"[{data.Name}]",
				ConfirmButtonText = _translations.Translate("General.Confirmation.ConfirmButton"),
				CancelButtonText = _translations.Translate("General.Confirmation.CancelButton")
			};
			var result = new ConfirmationDialog(dialogConfig).ShowDialog();

			if (result == true)
			{
				_configurations.RemoveLoadoutConfigById(data.Id);
				LoadoutConfigurations = new ObservableCollection<LoadoutConfiguration>(_configurations.Configurations.LoadoutConfigurations);
				OnPropertyChanged(nameof(LoadoutConfigurations));
			}
		}
	}
}
