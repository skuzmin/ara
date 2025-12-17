using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ARA.Controls
{
	public class AraButton : Button
	{

		public static readonly DependencyProperty HoverForegroundProperty =
			DependencyProperty.Register(
				nameof(HoverForeground),
				typeof(Brush),
				typeof(AraButton),
				new PropertyMetadata(Brushes.White));
		public Brush HoverForeground
		{
			get => (Brush)GetValue(HoverForegroundProperty);
			set => SetValue(HoverForegroundProperty, value);
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

		public static readonly DependencyProperty HoverBorderBrushProperty =
			DependencyProperty.Register(
				nameof(HoverBorderBrush),
				typeof(Brush),
				typeof(AraButton),
				new PropertyMetadata(Brushes.Transparent));
		public Brush HoverBorderBrush
		{
			get => (Brush)GetValue(HoverBorderBrushProperty);
			set => SetValue(HoverBorderBrushProperty, value);
		}

		public static readonly DependencyProperty SelectedBackgroundProperty =
			DependencyProperty.Register(
				nameof(SelectedBackground),
				typeof(Brush),
				typeof(AraButton),
				new PropertyMetadata(Brushes.DarkGray));
		public Brush SelectedBackground
		{
			get => (Brush)GetValue(SelectedBackgroundProperty);
			set => SetValue(SelectedBackgroundProperty, value);
		}

		public static readonly DependencyProperty SelectedForegroundProperty =
			DependencyProperty.Register(
				nameof(SelectedForeground),
				typeof(Brush),
				typeof(AraButton),
				new PropertyMetadata(Brushes.White));
		public Brush SelectedForeground
		{
			get => (Brush)GetValue(SelectedForegroundProperty);
			set => SetValue(SelectedForegroundProperty, value);
		}

		public static readonly DependencyProperty SelectedBorderBrushProperty =
			DependencyProperty.Register(
				nameof(SelectedBorderBrush),
				typeof(Brush),
				typeof(AraButton),
				new PropertyMetadata(Brushes.Transparent));
		public Brush SelectedBorderBrush
		{
			get => (Brush)GetValue(SelectedBorderBrushProperty);
			set => SetValue(SelectedBorderBrushProperty, value);
		}

		public static readonly DependencyProperty CornerRadiusProperty =
			DependencyProperty.Register(
				nameof(CornerRadius),
				typeof(CornerRadius),
				typeof(AraButton),
				new PropertyMetadata(new CornerRadius(0)));
		public CornerRadius CornerRadius
		{
			get => (CornerRadius)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		public static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register(
				nameof(IsSelected),
				typeof(bool),
				typeof(AraButton),
				new PropertyMetadata(false));
		public bool IsSelected
		{
			get => (bool)GetValue(IsSelectedProperty);
			set => SetValue(IsSelectedProperty, value);
		}


		static AraButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AraButton),
				new FrameworkPropertyMetadata(typeof(AraButton)));
		}
	}
}
