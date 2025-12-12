using System.Configuration;
using System.Data;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace ARA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
		}
	}

}
