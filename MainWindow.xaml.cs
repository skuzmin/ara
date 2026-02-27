using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ARA.Controls.CustomControls;
using ARA.Enums;
using ARA.ViewModels.Shell;
using Microsoft.Extensions.Logging;

namespace ARA
{
	public partial class MainWindow : AraWindow
	{
		public required AraButton ActiveButton;
		private readonly MainViewModel _vm;

		public MainWindow(MainViewModel vm, ILogger logger)
		{
			_vm = vm;
			InitializeComponent();
			DataContext = vm;
			Loaded += InitPillPosition;
			Cursor = App.AppCursor;
			logger.LogInformation("App Start");
		}

		public void NavigateFromTray(AraPage page)
		{
			var button = NavbarGrid.Children
				.OfType<AraButton>()
				.FirstOrDefault(b => (AraPage)b.Tag == page);

			button?.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
		}

		private void Navigation_Click(object sender, RoutedEventArgs e)
		{
			var button = (AraButton)sender;
			if (ActiveButton == button)
			{
				return;
			}

			var page = (AraPage)button.Tag;
			var result = _vm.Navigation.TryNavigateToPage(page);
			if (!result)
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

		private void InitPillPosition(object sender, RoutedEventArgs e)
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