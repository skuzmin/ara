using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ARA.Controls.UserControls
{
	public partial class TitleBarUserControl : UserControl
	{
		public TitleBarUserControl()
		{
			InitializeComponent();
		}

		private void WindowDrag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			base.OnPreviewMouseLeftButtonDown(e);
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				var window = Window.GetWindow(this);
				window?.DragMove();
			}
		}

		private void Minimize_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this)?.WindowState = WindowState.Minimized;
		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this)?.Close();
		}
	}
}
