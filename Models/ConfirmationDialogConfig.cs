namespace ARA.Models
{
	public class ConfirmationDialogConfig
	{
		public double MaxWidth { get; set; } = 480d;
		public string Title { get; set; } = "";
		public string CancelButtonText { get; set; } = "";
		public string ConfirmButtonText { get; set; } = "";
		public string Message { get; set; } = "";
		public string SubMessage { get; set; } = "";
	}
}
