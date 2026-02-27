using System.Diagnostics;
using System.Windows.Controls;

namespace ARA.Views
{
	public partial class AboutView : UserControl
	{
		public AboutView()
		{
			InitializeComponent();
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			var url = e.Uri.AbsoluteUri;
			Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
			e.Handled = true;
		}
	}
}
