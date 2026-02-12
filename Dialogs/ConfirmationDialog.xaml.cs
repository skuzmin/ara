using System.Windows;
using ARA.Models;

namespace ARA.Dialogs
{

	public partial class ConfirmationDialog : Window
	{
		public ConfirmationDialog(ConfirmationDialogConfig config)
		{
			InitializeComponent();
			MaxWidth = config.MaxWidth;
			Title = config.Title;
			MessageEl.Text = config.Message;
			ConfirmTextEl.Text = config.ConfirmButtonText;
			CancelTextEl.Text = config.CancelButtonText;
		}

		private void ConfirmButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
	}
}
