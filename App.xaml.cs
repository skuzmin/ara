using System.Windows;
using System.Windows.Input;
using ARA.Helpers;
using Hardcodet.Wpf.TaskbarNotification;

namespace ARA
{
    public partial class App : Application
    {
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

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

			Mouse.OverrideCursor = CursorHelper.CreateCursorFromPng("pack://application:,,,/Assets/cursor.png", 0, 0);
		}
	}

}
