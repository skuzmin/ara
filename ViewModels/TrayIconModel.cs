using System.Windows;
using System.Windows.Input;
using ARA.Enums;
using ARA.Interfaces;

namespace ARA.ViewModels
{
	public class TrayIconModel
	{
		public ICommand ShowWindowCommand { get; }
		public ICommand NavigateToPageCommand { get; }
		public ICommand ExitCommand { get; }
		public TrayIconModel(IAraNavigation navigation)
		{
			ShowWindowCommand = new RelayCommand(_ => ShowWindow());
			ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
			NavigateToPageCommand = new RelayCommand(
				page => NavigateToPage((AraPage)page),
				canExecute: page => navigation.CanNavigateToPage((AraPage)page));
		}

		private static void NavigateToPage(AraPage page)
		{
			var _window = (MainWindow)Application.Current.MainWindow;
			if (_window.Visibility != Visibility.Visible ||
				_window.WindowState == WindowState.Minimized)
			{
				ShowWindow();
			}

			_window.NavigateFromTray(page);
		}

		private static void ShowWindow()
		{
			Application.Current.MainWindow.Show();
			Application.Current.MainWindow.WindowState = WindowState.Normal;
			Application.Current.MainWindow.Activate();
		}
	}
}
