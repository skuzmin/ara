using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ARA.Enums;
using ARA.Helpers;
using ARA.Interfaces;
using ARA.Models;
using Microsoft.Extensions.Logging;

namespace ARA.ViewModels.Pages
{
	public class LoadoutViewModel : ViewModelBase
	{
		private readonly ILogger _logger;
		private readonly ScreenCoordinates _coordinates;
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
			_coordinates = config.GetSettingsConfiguration().Coordinates;
			Navigation = navigation;
			SelectedLoadout = null;
			CheckLoadout = new RelayCommand(OnCheckLoadoutClicked);
			LoadoutOptions = new ObservableCollection<LoadoutConfiguration>(config.Configurations.LoadoutConfigurations);
		}
		private async void OnCheckLoadoutClicked(object obj)
		{
			if (SelectedLoadout == null)
			{
				return;
			}
			var window = Application.Current.MainWindow;
			var iconPath = SelectedLoadout.Items.Select(item => item.Path).ToArray();

			//window.WindowState = WindowState.Minimized;
			//Dictionary<string, bool> results = await Task.Run(() => LoadoutCheckerHelper.CheckIcons(
			//	(int)SelectedLoadout.Coordinates.X,
			//	(int)SelectedLoadout.Coordinates.Y,
			//	(int)SelectedLoadout.Coordinates.Width,
			//	(int)SelectedLoadout.Coordinates.Height,
			//	iconPath));
			//window.WindowState = WindowState.Normal;

			Dictionary<string, bool> results = LoadoutCheckerHelper.CheckIcons(
				(int)_coordinates.X,
				(int)_coordinates.Y,
				(int)_coordinates.Width,
				(int)_coordinates.Height,
				iconPath);

			var newItems = SelectedLoadout.Items.Select(item => new GameItem
			{
				Icon = item.Icon,
				Quantity = item.Quantity,
				Status = results[item.Path] ? GameItemStatus.Success : GameItemStatus.Fail
			})
			.OrderByDescending(item => item.Status)
			.ToList();

			SelectedLoadout.Items = newItems;
			_logger.LogInformation("Loadout Check");
			OnPropertyChanged(nameof(SelectedLoadout));
		}
	}
}
