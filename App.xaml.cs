using System.Windows;
using System.Windows.Input;
using ARA.Helpers;
using ARA.Interfaces;
using ARA.Services;
using ARA.ViewModels;
using ARA.ViewModels.Pages;
using ARA.ViewModels.Shell;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ARA
{
	public partial class App : Application
	{
		private static Mutex? _mutex = null;
		private IServiceProvider? _serviceProvider;
		public static Cursor? AppCursor { get; private set; }
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			SingleInstanceChecker();
			FilesHelper.FilesDirectoryInit();

			var serviceCollection = new ServiceCollection();
			ConfigureServices(serviceCollection);
			_serviceProvider = serviceCollection.BuildServiceProvider();

			TrayIconInit();
			AppCursor = CursorHelper.CreateCursorFromPng("pack://application:,,,/Assets/Cursors/Cursor.png", 0, 0);

			var themes = _serviceProvider.GetService<IAraThemes>()!;
			themes.ActivateTheme();
			themes.ThemeChanged += ReloadMainWindow;

			_serviceProvider.GetService<IAraTranslation>()!.SetLocale();

			_serviceProvider.GetService<MainWindow>()!.Show();
		}

		private void ReloadMainWindow()
		{
			var oldWindow = Application.Current.MainWindow;
			var newWindow = _serviceProvider!.GetRequiredService<MainWindow>()!;
			var hotKeyService = _serviceProvider!.GetRequiredService<GlobalHotKeyService>()!;
			newWindow.Left = oldWindow.Left;
			newWindow.Top = oldWindow.Top;

			Application.Current.MainWindow = newWindow;
			newWindow.Show();
			oldWindow?.Close();
			hotKeyService.Register(newWindow);
		}

		private static void ConfigureServices(IServiceCollection services)
		{
			// Services
			services.AddSingleton<IAraConfigurations, ConfigurationService>();
			services.AddSingleton<ILogger, AraLogger>();
			services.AddSingleton<IAraThemes, ThemesService>();
			services.AddSingleton<IAraTranslation, TranslationService>();
			services.AddSingleton<IAraNavigation, NavigationService>();
			services.AddSingleton<IMainWindow, MainWindowService>();
			services.AddSingleton<GlobalHotKeyService>();
			services.AddSingleton<ILoadoutCheckerService, LoadoutCheckerService>();
			// ViewModels
			services.AddSingleton<MainViewModel>();
			services.AddSingleton<TrayIconViewModel>();
			services.AddTransient<LoadoutViewModel>();
			services.AddTransient<LoadoutConfigsViewModel>();
			services.AddTransient<LoadoutConfigDetailsViewModel>();
			services.AddTransient<SettingsViewModel>();
			services.AddTransient<AboutViewModel>();
			// Views
			services.AddTransient<MainWindow>();
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
			if (_serviceProvider == null)
			{
				return;
			}

			if (Resources["TrayIcon"] is TaskbarIcon trayIcon)
			{
				var navigationService = _serviceProvider.GetService<IAraNavigation>();
				var mainWindowService = _serviceProvider.GetService<IMainWindow>();
				trayIcon.Visibility = Visibility.Visible;
				trayIcon.DataContext = new TrayIconViewModel(navigationService!, mainWindowService!);
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
