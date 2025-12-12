using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ARA.Controls
{
	public class AraButton : Button
	{
		static AraButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(AraButton),
				new FrameworkPropertyMetadata(typeof(AraButton)));

			PaddingProperty.OverrideMetadata(
				typeof(AraButton),
				new FrameworkPropertyMetadata(new Thickness(12, 4, 12, 4)));

			MarginProperty.OverrideMetadata(
				typeof(AraButton),
				new FrameworkPropertyMetadata(new Thickness(0)));
		}

		public static readonly DependencyProperty HoverBackgroundProperty =
			DependencyProperty.Register(
				nameof(HoverBackground),
				typeof(Brush),
				typeof(AraButton),
				new PropertyMetadata(Brushes.DarkGray));

		public Brush HoverBackground
		{
			get => (Brush)GetValue(HoverBackgroundProperty);
			set => SetValue(HoverBackgroundProperty, value);
		}
	}
}
