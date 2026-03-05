using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using ARA.Dialogs;
using ARA.Interfaces;
using ARA.Models;
using ARA.Views;

namespace ARA.ViewModels.Pages
{
	public class SettingsViewModel : ViewModelBase
	{
		private readonly IAraThemes _themes;
		private readonly IAraTranslation _translations;
		private readonly IMainWindow _windowService;
		private readonly IAraConfigurations _configurations;
		public bool IsEdited
		{
			get => field;
			set
			{
				field = value;
				OnPropertyChanged(nameof(IsEdited));
			}
		}
		public List<SettingsItem> Themes { get; set; }
		public List<SettingsItem> Locales { get; set; }
		public double? CoordinatesX
		{
			get => field;
			set
			{
				field = value;
				IsEdited = true;
				OnPropertyChanged(nameof(CoordinatesX));
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
			}
		}
		public SettingsItem? SelectedLocale
		{
			get => field;
			set
			{
				field = value ;
				IsEdited = true;
				OnPropertyChanged(nameof(SelectedLocale));
			}
		}
		public SettingsItem? SelectedTheme
		{
			get => field;
			set
			{
				field = value;
				IsEdited = true;
				OnPropertyChanged(nameof(SelectedTheme));
			}
		}
		public ICommand SelectRegionCommand { get; }
		public ICommand OpenConfigFolderCommand { get; }
		public ICommand SaveSettingsCommand { get; }
		public SettingsViewModel(IAraThemes themes, IAraTranslation translations, IAraConfigurations configurations, IMainWindow windowService)
		{
			_themes = themes;
			_translations = translations;
			_windowService = windowService;
			_configurations = configurations;
			OpenConfigFolderCommand = new RelayCommand(_ => Process.Start("explorer.exe", Path.GetDirectoryName(Constants.ConfigFilePath)!));
			SaveSettingsCommand = new RelayCommand(_ => SaveSettings());
			SelectRegionCommand = new RelayCommand(_ => SelectRegion());
			Themes = _themes.GetThemes();
			Locales = _translations.GetLocales();
			_translations.TranslationChanged += UpdateTranslations;
			InitCoordinates();
			UpdateTranslations();
		}

		private void InitCoordinates()
		{
			var settings = _configurations.GetSettingsConfiguration();
			CoordinatesX = settings.Coordinates.X;
			CoordinatesY = settings.Coordinates.Y;
			CoordinatesHeight = settings.Coordinates.Height;
			CoordinatesWidth = settings.Coordinates.Width;
		}

		private void UpdateTranslations()
		{
			Themes.ForEach(t => t.Name = _translations.Translate(t.TranslationKey));
			Locales.ForEach(l => l.Name = _translations.Translate(l.TranslationKey));
			SelectedTheme = null;
			SelectedTheme = _themes.GetTheme();
			SelectedLocale = null;
			SelectedLocale = _translations.GetLocale();
			OnPropertyChanged(nameof(Themes));
			OnPropertyChanged(nameof(Locales));
			IsEdited = false;
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

		private void SaveSettings()
		{
			var settings = _configurations.GetSettingsConfiguration();
			settings.Coordinates = new ScreenCoordinates(CoordinatesX, CoordinatesY, CoordinatesHeight, CoordinatesWidth);
			_themes.UpdateTheme(SelectedTheme!);
			_translations.UpdateLocale(SelectedLocale!);
			_configurations.UpdateSettings(settings);
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
	}
}
