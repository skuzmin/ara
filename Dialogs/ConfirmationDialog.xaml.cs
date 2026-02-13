using System.Windows;
using ARA.Models;

namespace ARA.Dialogs
{

	public partial class ConfirmationDialog : AraWindow
	{
		public ConfirmationDialog(ConfirmationDialogConfig config)
		{
			InitializeComponent();
			MaxWidth = config.MaxWidth;
			Title = config.Title;
			MainMessageEl.Text = config.Message;
			SubMessageEl.Text = config.SubMessage;
			ConfirmTextEl.Text = config.ConfirmButtonText;
			CancelTextEl.Text = config.CancelButtonText;
		}

		private void ConfirmButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
