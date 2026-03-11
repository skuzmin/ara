using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ARA.Controls.CustomControls;
using ARA.Enums;
using ARA.Interfaces;
using ARA.Services;
using ARA.ViewModels.Shell;
using Microsoft.Extensions.Logging;

namespace ARA
{
	public partial class MainWindow : AraWindow
	{
		public required AraButton ActiveButton;
		private readonly ILoadoutCheckerService _loadoutCheckerService;
		private readonly GlobalHotKeyService _hotkeysService;
		private readonly MainViewModel _vm;

		public MainWindow(MainViewModel vm,
			ILogger logger,
			IAraTranslation translation,
			ILoadoutCheckerService loadoutChecker,
			GlobalHotKeyService hotkeys)
		{
			_vm = vm;
			_loadoutCheckerService = loadoutChecker;
			_hotkeysService = hotkeys;
			InitializeComponent();
			DataContext = vm;
			Loaded += OnWindowLoaded;
			translation.TranslationChanged += ReloadPill;
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

		private void OnWindowLoaded(object sender, RoutedEventArgs e)
		{
			InitPillPosition();
			_loadoutCheckerService.InitGameWindow();
			_hotkeysService.Register(this);
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

		private void ReloadPill()
		{
			Application.Current.Dispatcher.InvokeAsync(() =>
			{
				var selectedButton = NavbarGrid.Children.OfType<AraButton>().FirstOrDefault(b => b.IsSelected);
				if (selectedButton == null)
				{
					return;
				}

				InitPillPosition();
				selectedButton.UpdateLayout();

			}, System.Windows.Threading.DispatcherPriority.Render);
		}

		private void InitPillPosition()
		{
			var selectedButton = NavbarGrid.Children.OfType<AraButton>().FirstOrDefault(b => b.IsSelected);
			if (selectedButton == null)
			{
				return;
			}

			Point position = selectedButton.TransformToAncestor(NavbarGrid).Transform(new Point(0, 0));
			SelectionPill.Width = selectedButton.ActualWidth;
			SelectionPill.RenderTransform = new TranslateTransform(position.X, 0);
			ActiveButton = selectedButton;
		}
	}
}