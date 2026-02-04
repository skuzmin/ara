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
		private readonly string _configFilePath;
		private readonly ILogger _logger;
		private AraConfigurations _configurations;
		public AraConfigurations Configurations => _configurations;

		public ConfigurationService(ILogger logger)
		{
			_logger = logger;
			string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			_configFilePath = Path.Combine(appDataPath, "ARA", "configurations.json");
			_configurations = new AraConfigurations();
			_jsonOptions = new JsonSerializerOptions
			{
				WriteIndented = true
			};
			InitConfig();
		}

		public void InitConfig()
		{
			using (_logger.BeginScope(new { SccopeId = 111 }))
			{
                _logger.LogInformation("Test {a} - Test {b} lololo", 1, 2);
				try
				{
					string? directory = Path.GetDirectoryName(_configFilePath);
					if (!string.IsNullOrEmpty(directory))
					{
						Directory.CreateDirectory(directory);
					}
					else
					{
						throw new ArgumentException("Invalid config path!");
					}
					_logger.LogCritical("pewpew");

					if (File.Exists(_configFilePath))
					{
						string json = File.ReadAllText(_configFilePath);
						_configurations = JsonSerializer.Deserialize<AraConfigurations>(json) ?? throw new InvalidOperationException("Config file is corrupted!");
					}
					else
					{
						SaveConfig();
					}
					_logger.LogError("Error");
				}
				catch (Exception ex)
				{
					SaveConfig();
					//throw new InvalidOperationException($"Failed to initialize configuration: {ex.Message}", ex);
				}
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
			File.WriteAllText(_configFilePath, json);
		}
	}
}
