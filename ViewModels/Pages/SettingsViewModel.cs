using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ARA.Dialogs;
using ARA.Interfaces;
using ARA.Models;

namespace ARA.ViewModels.Pages
{
	public class SettingsViewModel : ViewModelBase
	{
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
		public SettingsItem SelectedLanguage
		{
			get => field;
			set
			{
				field = value;
				IsEdited = true;
				OnPropertyChanged(nameof(SelectedLanguage));
			}
		}
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
		public SettingsViewModel(IAraConfigurations configurations)
		{
			Init(configurations.GetSettingsConfiguration());
			_configurations = configurations;
			OpenConfigFolderCommand = new RelayCommand(_ => Process.Start("explorer.exe", Path.GetDirectoryName(Constants.ConfigFilePath)!));
			SaveSettingsCommand = new RelayCommand(_ => SaveSettings());
			IsEdited = false;
		}

		[MemberNotNull(nameof(SelectedLanguage))]
		[MemberNotNull(nameof(SelectedTheme))]
		private void Init(SettingsConfiguration config)
		{
			SelectedLanguage = Constants.Languages.FirstOrDefault(l => l.Id == config.Language) ?? Constants.Languages[0];
			SelectedTheme = Constants.Themes.FirstOrDefault(l => l.Id == config.Theme) ?? Constants.Themes[0];
		}

		private void SaveSettings()
		{
			IsEdited = false;
			_configurations.UpdateSettings(new SettingsConfiguration() { Language = SelectedLanguage.Id, Theme = SelectedTheme.Id });
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
