using System.Collections.ObjectModel;
using System.Windows.Input;
using ARA.Enums;
using ARA.Interfaces;
using ARA.Models;
using Microsoft.Extensions.Logging;

namespace ARA.ViewModels.Pages
{
	public class LoadoutViewModel : ViewModelBase
	{
		private readonly ILogger _logger;
		private readonly IAraConfigurations _config;
		public IAraNavigation Navigation { get; }
		public ICommand CheckLoadout { get; }
		public ObservableCollection<LoadoutConfiguration> LoadoutOptions { get; }
		public LoadoutConfiguration? SelectedLoadout
		{
			get => field;
			set
			{
				if (value != null)
				{
					_logger.LogInformation("Change loadout to: {loadout}", value.Name);
					value.Items.ForEach(x => x.Status = 0);
				}
				field = value;
				OnPropertyChanged(nameof(SelectedLoadout));
			}
		}

		public LoadoutViewModel(IAraConfigurations config, ILogger logger, IAraNavigation navigation)
		{
			_logger = logger;
			_config = config;
			Navigation = navigation;
			SelectedLoadout = null;
			CheckLoadout = new RelayCommand(OnCheckLoadoutClicked);
			LoadoutOptions = new ObservableCollection<LoadoutConfiguration>(config.Configurations.LoadoutConfigurations);
		}
		private void OnCheckLoadoutClicked(object obj)
		{
			if (SelectedLoadout == null)
			{
				return;
			}

			var newItems = SelectedLoadout.Items.Select(item => new GameItem
			{
				Icon = item.Icon,
				Quantity = item.Quantity,
				Status = (GameItemStatus)new Random().Next(1, 3)
			})
			.OrderByDescending(item => item.Status)
			.ToList();

			SelectedLoadout.Items = newItems;
			_logger.LogInformation("Loadout Check");
			OnPropertyChanged(nameof(SelectedLoadout));
		}
	}
}
