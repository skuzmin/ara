using System.Windows;
using System.Windows.Media;

namespace ARA
{
	public class AraWindow : Window
	{
		#region ClipRect
		public static readonly DependencyProperty ClipRectProperty =
			DependencyProperty.Register(
				nameof(ClipRect),
				typeof(Rect),
				typeof(AraWindow),
				new FrameworkPropertyMetadata(
					Rect.Empty,
					FrameworkPropertyMetadataOptions.AffectsRender));
		public Rect ClipRect
		{
			get => (Rect)GetValue(ClipRectProperty);
			set => SetValue(ClipRectProperty, value);
		}
		#endregion
		#region MinimizeButtonDisplay
		public static readonly DependencyProperty MinimizeButtonDisplayProperty =
			DependencyProperty.Register(
				nameof(MinimizeButtonDisplay),
				typeof(Visibility),
				typeof(AraWindow),
				new FrameworkPropertyMetadata(
					Visibility.Visible,
					FrameworkPropertyMetadataOptions.AffectsRender));
		public Visibility MinimizeButtonDisplay
		{
			get => (Visibility)GetValue(MinimizeButtonDisplayProperty);
			set => SetValue(MinimizeButtonDisplayProperty, value);
		}
		#endregion
		#region TitleBarBackground
		public static readonly DependencyProperty TitleBarBackgroundProperty =
			DependencyProperty.Register(
				nameof(TitleBarBackground),
				typeof(Brush),
				typeof(AraWindow),
				new FrameworkPropertyMetadata(
					Brushes.Transparent,
					FrameworkPropertyMetadataOptions.AffectsRender));
		public Brush TitleBarBackground
		{
			get => (Brush)GetValue(TitleBarBackgroundProperty);
			set => SetValue(TitleBarBackgroundProperty, value);
		}
		#endregion
		static AraWindow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AraWindow),
				new FrameworkPropertyMetadata(typeof(AraWindow)));
		}
        public AraWindow()
        {
			Loaded += OnContentLoaded;
		}

		private void OnContentLoaded(object sender, EventArgs e)
		{
			ClipRect = new Rect(0, 0, ActualWidth, ActualHeight);
		}
	}
}
