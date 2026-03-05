using System.Windows;
using ARA.Interfaces;

namespace ARA.Services
{
	public class MainWindowService : IMainWindow
	{
		public void HideMainWindow()
		{
			Application.Current.MainWindow.Hide();
		}

		public void ShowMainWindow()
		{
			Application.Current.MainWindow.Show();
			Application.Current.MainWindow.WindowState = WindowState.Normal;
			Application.Current.MainWindow.Activate();
		}
	}
}
