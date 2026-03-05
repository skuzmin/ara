using System.Windows;
using System.Windows.Input;
using ARA.Enums;
using ARA.Interfaces;

namespace ARA.ViewModels
{
	public class TrayIconViewModel
	{
		private readonly IMainWindow _mainWindow;
		public ICommand ShowWindowCommand { get; }
		public ICommand NavigateToPageCommand { get; }
		public ICommand ExitCommand { get; }
		public TrayIconViewModel(IAraNavigation navigation, IMainWindow window)
		{
			_mainWindow = window;
			ShowWindowCommand = new RelayCommand(_ => _mainWindow.ShowMainWindow());
			ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
			NavigateToPageCommand = new RelayCommand(
				page => NavigateToPage((AraPage)page),
				canExecute: page => navigation.CanNavigateToPage((AraPage)page));
		}

		private void NavigateToPage(AraPage page)
		{
			var _window = (MainWindow)Application.Current.MainWindow;
			if (_window.Visibility != Visibility.Visible ||
				_window.WindowState == WindowState.Minimized)
			{
				_mainWindow.ShowMainWindow();
			}
			_window.NavigateFromTray(page);
		}
	}
}
