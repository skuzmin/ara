using System.Windows;

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
