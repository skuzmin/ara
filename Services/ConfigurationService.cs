using System.IO;
using System.Text.Json;
using ARA.Interfaces;
using ARA.Models;
using Microsoft.Extensions.Logging;

namespace ARA.Services
{
	class ConfigurationService : IAraConfigurations
	{
		private readonly JsonSerializerOptions _jsonOptions;
		private readonly ILogger _logger;
		private AraConfigurations _configurations;
		private LoadoutConfiguration? _loadoutConfiguration;
		public AraConfigurations Configurations => _configurations;

		public ConfigurationService(ILogger logger)
		{
			_logger = logger;
			_configurations = new AraConfigurations();
			_jsonOptions = new JsonSerializerOptions
			{
				WriteIndented = true
			};
			InitConfig();
		}

		#region General
		public void SaveConfig()
		{
			_configurations.LoadoutConfigurations.Sort((a, b) => a.Name.CompareTo(b.Name));
			string json = JsonSerializer.Serialize(_configurations, _jsonOptions);
			File.WriteAllText(Constants.ConfigFilePath, json);
		}

		public void InitConfig()
		{
			try
			{
				if (File.Exists(Constants.ConfigFilePath))
				{
					string json = File.ReadAllText(Constants.ConfigFilePath);
					_configurations = JsonSerializer.Deserialize<AraConfigurations>(json) ?? throw new InvalidOperationException("Config file is corrupted!");
				}
				else
				{
					SaveConfig();
				}
			}
			catch (Exception ex)
			{
				SaveConfig();
				_logger.LogError("Error during ConfigurationService Init: {Message}", ex.Message);
			}
			ValidateSettingsConfig();
			SetDebugLevel();
		}
		#endregion

		#region SettingsConfig
		private void ValidateSettingsConfig()
		{
			if (!Constants.Themes.Any(i => i.Id == _configurations.SettingsConfiguration.Theme))
			{
				_configurations.SettingsConfiguration.Theme = Constants.Themes[0].Id;
			}
			if (!Constants.Locales.Any(i => i.Id == _configurations.SettingsConfiguration.Locale))
			{
				_configurations.SettingsConfiguration.Locale = Constants.Locales[0].Id;
			}
			if (!Constants.DebugLevels.Any(i => i.Id == _configurations.SettingsConfiguration.DebugLevel))
			{
				_configurations.SettingsConfiguration.DebugLevel = Constants.DebugLevels[0].Id;
			}
		}

		private void SetDebugLevel()
		{
			(_logger as AraLogger)!.SetDebugLevel(_configurations.SettingsConfiguration.DebugLevel);
		}

		public SettingsConfiguration GetSettingsConfiguration()
		{
			return _configurations.SettingsConfiguration;
		}

		public void UpdateSettings(SettingsConfiguration settings)
		{
			_configurations.SettingsConfiguration = settings;
			SetDebugLevel();
			SaveConfig();
		}

		public SettingsItem GetDebugLevel()
		{
			return Constants.DebugLevels.FirstOrDefault(l => l.Id == _configurations.SettingsConfiguration.DebugLevel)!;
		}

		#endregion

		#region LoadoutConfig
		public LoadoutConfiguration? GetCurrentLoadoutConfig()
		{
			return _loadoutConfiguration;
		}

		public void SetCurrentLoadoutConfig(LoadoutConfiguration? loadout)
		{
			_loadoutConfiguration = loadout;
		}

		public void RemoveLoadoutConfigById(Guid id)
		{
			var itemToRemove = _configurations.LoadoutConfigurations.FirstOrDefault(x => x.Id == id);
			if (itemToRemove != null)
			{
				_configurations.LoadoutConfigurations.Remove(itemToRemove);
				_logger.LogInformation("Deleted Loadout configuration: {Name}", itemToRemove.Name);
				SaveConfig();
			}
			else
			{
				_logger.LogError("Failed to delete Configuration with Id: {Id}", id);
			}
		}

		public void CreateLoadoutConfig(LoadoutConfiguration loadout)
		{
			_logger.LogInformation("Create new Loadout: {loadout}", loadout.Name);
			_configurations.LoadoutConfigurations.Add(loadout);
			SaveConfig();
		}

		public void UpdateLoadoutConfig(LoadoutConfiguration loadout)
		{
			var index = _configurations.LoadoutConfigurations.FindIndex(x => x.Id == loadout.Id);
			if (index >= 0)
			{
				_logger.LogInformation("Update Loadout: {loadout}", loadout.Name);
				_configurations.LoadoutConfigurations[index] = loadout;
				SaveConfig();
			}
			else
			{
				_logger.LogError("Failed to update Configuration with Id: {Id}", loadout.Id);
			}
		}
		#endregion
	}
}
