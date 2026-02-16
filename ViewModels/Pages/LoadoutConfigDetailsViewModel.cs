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
		public ICommand BackCommand { get; }
		public ObservableCollection<GameItem> ItemsList { get; }
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

		public LoadoutConfigDetailsViewModel(IAraNavigation navigations, IAraConfigurations configurations, ILogger logger)
		{
			_logger = logger;
			BackCommand = new RelayCommand(_ => navigations.NavigateToPage(AraPage.LoadoutConfigs));
			ItemsList = new ObservableCollection<GameItem>(GameItem.GetList());
		}
	}
}
