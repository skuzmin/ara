using System.IO;
using System.Text.Json;
using ARA.Enums;
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


			_configurations.LoadoutConfigurations = [
				new LoadoutConfiguration { Name = "Dam Night Juice", Items = [
						new GameItem{Icon = GameIcon.Bandage, Quantity = 5 },
						new GameItem{Icon = GameIcon.Shield_Recharger, Quantity = 5 },
						new GameItem{Icon = GameIcon.Door_Blocker, Quantity = 3 },
						new GameItem{Icon = GameIcon.Heavy_Fuse_Grenade, Quantity = 2 },
						new GameItem{Icon = GameIcon.Adrenaline_Shot, Quantity = 3 },
					]
				},
				new LoadoutConfiguration { Name = "Blue Gate", Items = [
						new GameItem{Icon = GameIcon.Bandage, Quantity = 5 },
						new GameItem{Icon = GameIcon.Shield_Recharger, Quantity = 5 },
						new GameItem{Icon = GameIcon.Barricade_Kit, Quantity = 3 },
						new GameItem{Icon = GameIcon.Lil_Smoke_Grenade, Quantity = 2 },
						new GameItem{Icon = GameIcon.Adrenaline_Shot, Quantity = 3 },
					]
				},
				new LoadoutConfiguration { Name = "Stela Montis Poor", Items = [
						new GameItem{Icon = GameIcon.Bandage, Quantity = 3 },
						new GameItem{Icon = GameIcon.Shield_Recharger, Quantity = 3 },
						new GameItem{Icon = GameIcon.Lil_Smoke_Grenade, Quantity = 2 },
						new GameItem{Icon = GameIcon.Raider_Hatch_Key, Quantity = 1 },
					]
				}
			];
		}

		public void SaveConfig()
		{
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

		#region LoadoutConfig
		public LoadoutConfiguration GetCurrentLoadoutConfig()
		{
			return _loadoutConfiguration ?? new LoadoutConfiguration();
		}

		public void SetCurrentConfigurationById(Guid id)
		{
			var config = _configurations.LoadoutConfigurations.FirstOrDefault(x => x.Id == id);
			_loadoutConfiguration = config ?? new LoadoutConfiguration();
		}

		public void SetCurrentConfigurationAsNew()
		{
			_loadoutConfiguration = null;
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
				_logger.LogWarning("Fail to delete Configuration with Id: {Id}", id);
			}
		}
		#endregion
	}
}
