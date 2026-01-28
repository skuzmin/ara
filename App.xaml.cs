using System.Windows;
using System.Windows.Input;
using ARA.Helpers;
using Hardcodet.Wpf.TaskbarNotification;

namespace ARA
{
	public partial class App : Application
	{
		private static Mutex? _mutex = null;
		protected override void OnStartup(StartupEventArgs e)
		{
            SingleInstanceChecker();
			base.OnStartup(e);
			TrayIconInit();
			Mouse.OverrideCursor = CursorHelper.CreateCursorFromPng("pack://application:,,,/Assets/cursor.png", 0, 0);
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
	}
}
