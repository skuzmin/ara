using System.Collections.ObjectModel;
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
		private List<TextBlock> _textBlocks;

		#region Children
		public ObservableCollection<UIElement> Children
		{
			get { return (ObservableCollection<UIElement>)GetValue(ChildrenProperty); }
			set { SetValue(ChildrenProperty, value); }
		}

		public static readonly DependencyProperty ChildrenProperty =
		DependencyProperty.Register(
			nameof(Children),
			typeof(ObservableCollection<UIElement>),
			typeof(AraButton),
			new PropertyMetadata(null));
		#endregion
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
				new PropertyMetadata(false, OnIsSelectedChanged));
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
		public AraButton()
		{
			Children = [];
			_textBlocks = [];
		}

		#region Animations
		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var button = (AraButton)d;
			bool isSelected = (bool)e.NewValue;
			var targetColor = isSelected ? button.SelectedForeground : button.Foreground;
			foreach (var tb in button._textBlocks)
			{
				ColorAnimator.Animate(targetColor, tb, "(Foreground).(SolidColorBrush.Color)");
			}
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_border = (Border)GetTemplateChild("Border");
			_textBlocks = [.. Children.OfType<TextBlock>()];
			foreach (var tb in _textBlocks)
			{
				tb.Foreground = this.Foreground.Clone();
			}
		}

		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			if (!IsSelected)
			{
				ColorAnimator.Animate(HoverBackground, _border!, "(Background).(SolidColorBrush.Color)");
				ColorAnimator.Animate(HoverBorderBrush, _border!, "(BorderBrush).(SolidColorBrush.Color)");
				foreach (var tb in _textBlocks)
				{
					ColorAnimator.Animate(HoverForeground, tb, "(Foreground).(SolidColorBrush.Color)");
				}
			}
		}

		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			if (!IsSelected)
			{
				ColorAnimator.Animate(Background, _border!, "(Background).(SolidColorBrush.Color)");
				ColorAnimator.Animate(BorderBrush, _border!, "(BorderBrush).(SolidColorBrush.Color)");
				foreach (var tb in _textBlocks)
				{
					ColorAnimator.Animate(Foreground, tb, "(Foreground).(SolidColorBrush.Color)");
				}
			}
		}

		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnGotKeyboardFocus(e);
			if (!IsSelected && !IsMouseOver)
			{
				ColorAnimator.Animate(HoverBackground, _border!, "(Background).(SolidColorBrush.Color)");
				ColorAnimator.Animate(HoverBorderBrush, _border!, "(BorderBrush).(SolidColorBrush.Color)");
				foreach (var tb in _textBlocks)
				{
					ColorAnimator.Animate(HoverForeground, tb, "(Foreground).(SolidColorBrush.Color)");
				}
			}
		}

		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (!IsSelected && !IsMouseOver)
			{
				ColorAnimator.Animate(Background, _border!, "(Background).(SolidColorBrush.Color)");
				ColorAnimator.Animate(BorderBrush, _border!, "(BorderBrush).(SolidColorBrush.Color)");
				foreach (var tb in _textBlocks)
				{
					ColorAnimator.Animate(Foreground, tb, "(Foreground).(SolidColorBrush.Color)");
				}
			}
		}

		#endregion
	}
}
