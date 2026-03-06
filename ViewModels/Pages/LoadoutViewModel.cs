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
		private readonly IAraConfigurations _configurations;
		private readonly IMainWindow _window;
		private readonly ScreenCoordinates _coordinates;
		public IAraNavigation Navigation { get; }
		public ICommand CheckLoadout { get; }
		public bool IsDefaultZone { get; }
		public ObservableCollection<LoadoutConfiguration> LoadoutOptions { get; }
		public bool IsLoading
		{
			get => field;
			set
			{
				field = value;
				OnPropertyChanged(nameof(IsLoading));
			}
		}
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
		public LoadoutViewModel(IAraConfigurations config, ILogger logger, IAraNavigation navigation, IMainWindow window)
		{
			_logger = logger;
			_configurations = config;
			_window = window;
			_coordinates = config.GetSettingsConfiguration().Coordinates;
			IsDefaultZone = _coordinates.IsDefaultZone();
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
			IsLoading = true;
			_logger.LogInformation("Loadout Check: {Loadout}", SelectedLoadout.Name);
			var window = Application.Current.MainWindow;
			var results = new Dictionary<int, bool>();

			if (_configurations.IsCaptureModeIgnoreARA())
			{
				_window.HideMainWindow();
				var checkTask = Task.Run(() => LoadoutCheckerHelper.CheckIcons(
					(int)_coordinates.X,
					(int)_coordinates.Y,
					(int)_coordinates.Width,
					(int)_coordinates.Height,
					SelectedLoadout.Items,
					_logger));
				_window.ShowMainWindow();
				results = await checkTask;
			}
			else
			{
				results = await Task.Run(() => LoadoutCheckerHelper.CheckIcons(
					(int)_coordinates.X,
					(int)_coordinates.Y,
					(int)_coordinates.Width,
					(int)_coordinates.Height,
					SelectedLoadout.Items,
					_logger));
			}

			var newItems = SelectedLoadout.Items.Select(item => new GameItem
			{
				Icon = item.Icon,
				Quantity = item.Quantity,
				Status = results[item.Id] ? GameItemStatus.Success : GameItemStatus.Fail
			})
			.OrderByDescending(item => item.Status)
			.ToList();

			IsLoading = false;
			SelectedLoadout.Items = newItems;
			OnPropertyChanged(nameof(SelectedLoadout));
		}
	}
}
