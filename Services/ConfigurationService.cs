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

		}
		#endregion

		#region SettingsConfig
		public SettingsConfiguration GetSettingsConfiguration()
		{
			return _configurations.SettingsConfiguration;
		}

		public void UpdateSettings(SettingsConfiguration settings)
		{
			_configurations.SettingsConfiguration = settings;
			SaveConfig();
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
