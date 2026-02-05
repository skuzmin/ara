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

		public void UpdateConfig(AraConfigurations configurations)
		{
			_configurations = configurations;
			SaveConfig();
		}

		public void SaveConfig()
		{
			string json = JsonSerializer.Serialize(_configurations, _jsonOptions);
			File.WriteAllText(Constants.ConfigFilePath, json);
		}
	}
}
