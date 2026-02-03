using System.IO;
using System.Text.Json;
using ARA.Interfaces;
using ARA.Models;

namespace ARA.Services
{
	class ConfigurationService : IAraConfigurations
	{
		private readonly JsonSerializerOptions _jsonOptions;
		private readonly string _configFilePath;
		private AraConfigurations _configurations;
		public AraConfigurations Configurations => _configurations;

		public ConfigurationService()
		{
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
			try
			{
				string directory = Path.GetDirectoryName(_configFilePath) ?? throw new ArgumentException("Wrong config path!");
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

				if (File.Exists(_configFilePath))
				{
					string json = File.ReadAllText(_configFilePath);
					_configurations = JsonSerializer.Deserialize<AraConfigurations>(json) ?? throw new NullReferenceException("Config file is corrupted!");
				}
				else
				{
					SaveConfig();
				}
			}
			catch (Exception ex)
			{
				_configurations = new AraConfigurations();
				SaveConfig();
				// add logger
				//throw new InvalidOperationException($"Failed to initialize configuration: {ex.Message}", ex);
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
