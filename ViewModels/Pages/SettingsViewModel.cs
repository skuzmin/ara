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
		public SettingsItem SelectedLocale
		{
			get => field;
			set
			{
				field = value;
				IsEdited = true;
				OnPropertyChanged(nameof(SelectedLocale));
			}
		}
		public List<SettingsItem> Themes { get; set; }
		public List<SettingsItem> Locales { get; set; }
		public SettingsItem SelectedTheme
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
			SelectedTheme = _themes.GetTheme();
			SelectedLocale = _translations.GetLocale();
			Themes = _themes.GetThemes();
			Locales = _translations.GetLocales();
			IsEdited = false;
		}

		private void SaveSettings()
		{
			IsEdited = false;
			_themes.UpdateTheme(SelectedTheme);
			_translations.UpdateLocale(SelectedLocale);
		}

		public override bool CanNavigateAway()
		{
			if (!IsEdited)
			{
				return true;
			}

			var dialogConfig = new ConfirmationDialogConfig
			{
				Title = "Unsaved changes",
				Message = $"All unsaved changes will be lost.",
				SubMessage = "Are you sure you want to discard changes?",
				ConfirmButtonText = "Discard",
				CancelButtonText = "Cancel"
			};

			var result = new ConfirmationDialog(dialogConfig).ShowDialog();
			return result ?? false;
		}
	}
}
