using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using ARA.Dialogs;
using ARA.Interfaces;
using ARA.Models;

namespace ARA.ViewModels.Pages
{
	public class SettingsViewModel : ViewModelBase
	{
		private readonly IAraThemes _themes;
		private readonly IAraTranslation _translations;
		private readonly IAraConfigurations _configurations;
		private readonly ILoadoutCheckerService _loadoutChecker;
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
		public List<SettingsItem> DebugLevels { get; set; }
		public SettingsItem? SelectedLocale
		{
			get => field;
			set
			{
				field = value;
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
		public SettingsItem? SelectedDebugLevel
		{
			get => field;
			set
			{
				field = value;
				IsEdited = true;
				OnPropertyChanged(nameof(SelectedDebugLevel));
			}
		}
		public bool IsGameDetected
		{
			get => _loadoutChecker.IsGameDetected();
		}
		public ICommand OpenConfigFolderCommand { get; }
		public ICommand SaveSettingsCommand { get; }
		public ICommand DetectGameCommand { get; }
		public SettingsViewModel(IAraThemes themes,
			IAraTranslation translations,
			IAraConfigurations configurations,
			ILoadoutCheckerService loadoutChecker)
		{
			_themes = themes;
			_translations = translations;
			_configurations = configurations;
			_loadoutChecker = loadoutChecker;
			OpenConfigFolderCommand = new RelayCommand(_ => Process.Start("explorer.exe", Path.GetDirectoryName(Constants.ConfigFilePath)!));
			SaveSettingsCommand = new RelayCommand(_ => SaveSettings());
			DetectGameCommand = new RelayCommand(_ =>
			{
				loadoutChecker.CaptureGameWindow();
				OnPropertyChanged(nameof(IsGameDetected));
			});
			Themes = _themes.GetThemes();
			Locales = _translations.GetLocales();
			DebugLevels = Constants.DebugLevels;
			_translations.TranslationChanged += UpdateTranslations;
			UpdateTranslations();
		}

		private void UpdateTranslations()
		{
			Themes.ForEach(t => t.Name = _translations.Translate(t.TranslationKey));
			Locales.ForEach(l => l.Name = _translations.Translate(l.TranslationKey));
			DebugLevels.ForEach(l => l.Name = _translations.Translate(l.TranslationKey));
			SelectedTheme = null;
			SelectedLocale = null;
			SelectedDebugLevel = null;
			SelectedTheme = _themes.GetTheme();
			SelectedLocale = _translations.GetLocale();
			SelectedDebugLevel = _configurations.GetDebugLevel();
			IsEdited = false;
		}

		private void SaveSettings()
		{
			var settings = _configurations.GetSettingsConfiguration();
			settings.DebugLevel = SelectedDebugLevel!.Id;
			_themes.UpdateTheme(SelectedTheme!.Id);
			_translations.UpdateLocale(SelectedLocale!.Id);

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
