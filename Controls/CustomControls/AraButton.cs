using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ARA.Animations;

namespace ARA.Controls.CustomControls
{
	public class AraButton : Button
	{
		private Border? _border;

		#region HoverForeground
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
		#endregion
		#region HoverBackground
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
		#endregion
		#region HoverBorderBrush
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
		#endregion
		#region SelectedBackground
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
		#endregion
		#region SelectedForeground
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
		#endregion
		#region SelectedBorderBrush
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
		#endregion
		#region CornerRadius
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
		#endregion
		#region IsSelected
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
		#endregion

		static AraButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AraButton),
				new FrameworkPropertyMetadata(typeof(AraButton)));
		}

		#region Animations
		public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
			_border = (Border)GetTemplateChild("Border");
		}

		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			if (!IsSelected)
			{
				ColorAnimator.Animate(HoverBackground, _border!, "(Background).(SolidColorBrush.Color)");
				ColorAnimator.Animate(HoverBorderBrush, _border!, "(BorderBrush).(SolidColorBrush.Color)");
			}
		}

		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			if (!IsSelected)
			{
				ColorAnimator.Animate(Background, _border!, "(Background).(SolidColorBrush.Color)");
				ColorAnimator.Animate(BorderBrush, _border!, "(BorderBrush).(SolidColorBrush.Color)");
			}
		}
		#endregion
	}
}
