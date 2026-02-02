using System.Windows;
using System.Windows.Input;
using ARA.Helpers;
using ARA.Interfaces;
using ARA.Services;
using ARA.ViewModels.Pages;
using ARA.ViewModels.Shell;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.DependencyInjection;

namespace ARA
{
	public partial class App : Application
	{
		private static Mutex? _mutex = null;
		private IServiceProvider? _serviceProvider;
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			SingleInstanceChecker();

			var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
			_serviceProvider = serviceCollection.BuildServiceProvider();

			TrayIconInit();
			Mouse.OverrideCursor = CursorHelper.CreateCursorFromPng("pack://application:,,,/Assets/cursor.png", 0, 0);

			_serviceProvider.GetService<MainWindow>()!.Show();
		}

        private static void ConfigureServices(IServiceCollection services)
		{
			// Services
			services.AddSingleton<IConfigurations, ConfigurationService>();
			// ViewModels
			services.AddTransient<MainViewModel>();
			services.AddTransient<LoadoutViewModel>();
			services.AddTransient<SettingsViewModel>();
			services.AddTransient<AboutViewModel>();
			// Views
			services.AddSingleton<MainWindow>();
		}

		private static void SingleInstanceChecker()
		{
			_mutex = new Mutex(true, "ARA_ArcRaidersAssistant", out bool isFirstInstance);
			if (!isFirstInstance)
			{ 
				Application.Current.Shutdown();
				return;
			}
		}

		private void TrayIconInit()
		{
			if (Resources["TrayIcon"] is TaskbarIcon trayIcon)
			{
				trayIcon.Visibility = System.Windows.Visibility.Visible;
				trayIcon.TrayMouseDoubleClick += (s, args) =>
				{
					MainWindow?.Show();
					MainWindow!.WindowState = WindowState.Normal;
					MainWindow.Activate();
				};
			}
		}

        protected override void OnExit(ExitEventArgs e)
        {
			_mutex?.Dispose();
			if (_serviceProvider is IDisposable disposable)
			{
				disposable.Dispose();
			}
			base.OnExit(e);
        }
	}
}
