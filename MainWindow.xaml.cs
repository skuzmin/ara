using ARA.ViewModels;

namespace ARA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AraWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainViewModel();
		}
	}
}