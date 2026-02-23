using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARA.Interfaces;
using ARA.ViewModels.Pages;

namespace ARA.Views
{
	public partial class LoadoutConfigDetailsView : UserControl
	{
		public LoadoutConfigDetailsView()
		{
			InitializeComponent();
			LoadoutPreview.PreviewMouseWheel += DataGrid_PreviewMouseWheel;
			DataContextChanged += (s, e) =>
			{
				if (DataContext is LoadoutConfigDetailsViewModel vm)
				{
					vm.ResetComboBox = () => ItemsComboBox.Reset();
				}
			};
		}

		private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
				{
					RoutedEvent = UIElement.MouseWheelEvent,
					Source = sender
				};
				var parent = ((Control)sender).Parent as UIElement;
				parent?.RaiseEvent(eventArg);
			}
		}
	}
}
