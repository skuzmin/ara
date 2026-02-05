using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ARA.Controls.CustomControls;
using ARA.ViewModels.Shell;
using Microsoft.Extensions.Logging;

namespace ARA
{
	public partial class MainWindow : AraWindow
	{
		public required AraButton ActiveButton;

		public MainWindow(MainViewModel vm, ILogger logger)
		{
			InitializeComponent();
			DataContext = vm;
			Loaded += (s, e) => InitPillPosition();
			logger.LogInformation("App Start");
		}

		private void Navigation_Click(object sender, RoutedEventArgs e)
		{
			var button = (AraButton)sender;
			if (ActiveButton == button)
			{
				return;
			}
			ActiveButton = button;

			Point buttonPosition = button.TransformToAncestor(NavbarGrid).Transform(new Point(0, 0));

			var positionAnimation = new DoubleAnimation
			{
				To = buttonPosition.X,
				Duration = TimeSpan.FromMilliseconds(150),
				EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
			};
			var widthAnimation = new DoubleAnimation
			{
				To = button.ActualWidth,
				Duration = TimeSpan.FromMilliseconds(150),
				EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
			};

			SelectionPill.RenderTransform.BeginAnimation(TranslateTransform.XProperty, positionAnimation);
			SelectionPill.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);
		}

		private void InitPillPosition()
		{
			var selectedButton = NavbarGrid.Children.OfType<AraButton>().FirstOrDefault(b => b.IsSelected);
			if (selectedButton != null)
			{
				Point position = selectedButton.TransformToAncestor(NavbarGrid).Transform(new Point(0, 0));
				SelectionPill.Width = selectedButton.ActualWidth;
				SelectionPill.RenderTransform = new TranslateTransform(position.X, 0);
				ActiveButton = selectedButton;
			}
		}
	}
}