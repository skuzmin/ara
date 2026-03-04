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
		public ICommand OpenConfigFolderCommand { get; }
		public ICommand SaveSettingsCommand { get; }
		public SettingsViewModel(IAraThemes themes, IAraTranslation translations)
		{
			_themes = themes;
			_translations = translations;
			OpenConfigFolderCommand = new RelayCommand(_ => Process.Start("explorer.exe", Path.GetDirectoryName(Constants.ConfigFilePath)!));
			SaveSettingsCommand = new RelayCommand(_ => SaveSettings());
			Themes = _themes.GetThemes();
			Locales = _translations.GetLocales();
			_translations.TranslationChanged += UpdateTranslations;
			UpdateTranslations();
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

		private void SaveSettings()
		{
			_themes.UpdateTheme(SelectedTheme!);
			_translations.UpdateLocale(SelectedLocale!);
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
