using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ARA.Controls
{
	class AraComboBox : ComboBox
	{
		#region SelectedTextForegorund
		public static readonly DependencyProperty SelectedTextForegroundProperty =
			DependencyProperty.Register(
				nameof(SelectedTextForeground),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.White));
		public Brush SelectedTextForeground
		{
			get => (Brush)GetValue(SelectedTextForegroundProperty);
			set => SetValue(SelectedTextForegroundProperty, value);
		}
		#endregion
		#region SelectedTextFontSize
		public static readonly DependencyProperty SelectedTextFontSizeProperty =
			DependencyProperty.Register(
				nameof(SelectedTextFontSize),
				typeof(Double),
				typeof(AraComboBox),
				new PropertyMetadata(16.0));
		public Double SelectedTextFontSize
		{
			get => (Double)GetValue(SelectedTextFontSizeProperty);
			set => SetValue(SelectedTextFontSizeProperty, value);
		}
		#endregion
		#region SelectedTextFontFamily
		public static readonly DependencyProperty SelectedTextFontFamilyProperty =
			DependencyProperty.Register(
				nameof(SelectedTextFontFamily),
				typeof(FontFamily),
				typeof(AraComboBox),
				new PropertyMetadata(new FontFamily("Segoe UI")));
		public FontFamily SelectedTextFontFamily
		{
			get => (FontFamily)GetValue(SelectedTextFontFamilyProperty);
			set => SetValue(SelectedTextFontFamilyProperty, value);
		}
		#endregion
		#region SelectedTextFontWeight
		public static readonly DependencyProperty SelectedTextFontWeightProperty =
			DependencyProperty.Register(
				nameof(SelectedTextFontWeight),
				typeof(FontWeight),
				typeof(AraComboBox),
				new PropertyMetadata(FontWeights.Normal));
		public FontWeight SelectedTextFontWeight
		{
			get => (FontWeight)GetValue(SelectedTextFontWeightProperty);
			set => SetValue(SelectedTextFontWeightProperty, value);
		}
		#endregion
		#region SelectedTextMargin
		public static readonly DependencyProperty SelectedTextMarginProperty =
			DependencyProperty.Register(
				nameof(SelectedTextMargin),
				typeof(Thickness),
				typeof(AraComboBox),
				new PropertyMetadata(new Thickness(0)));
		public Thickness SelectedTextMargin
		{
			get => (Thickness)GetValue(SelectedTextMarginProperty);
			set => SetValue(SelectedTextMarginProperty, value);
		}
		#endregion

		#region ItemsListCornerRadius
		public static readonly DependencyProperty ItemsListCornerRadiusProperty =
			DependencyProperty.Register(
				nameof(ItemsListCornerRadius),
				typeof(CornerRadius),
				typeof(AraComboBox),
				new PropertyMetadata(new CornerRadius(0)));
		public CornerRadius ItemsListCornerRadius
		{
			get => (CornerRadius)GetValue(ItemsListCornerRadiusProperty);
			set => SetValue(ItemsListCornerRadiusProperty, value);
		}
		#endregion
		#region ItemsListMaxHeight
		public static readonly DependencyProperty ItemsListMaxHeightProperty =
			DependencyProperty.Register(
				nameof(ItemsListMaxHeight),
				typeof(Double),
				typeof(AraComboBox),
				new PropertyMetadata(120.0));
		public Double ItemsListMaxHeight
		{
			get => (Double)GetValue(ItemsListMaxHeightProperty);
			set => SetValue(ItemsListMaxHeightProperty, value);
		}
		#endregion

		#region ListItemHeight
		public static readonly DependencyProperty ListItemHeightProperty =
			DependencyProperty.Register(
				nameof(ListItemHeight),
				typeof(Double),
				typeof(AraComboBox),
				new PropertyMetadata(40.0));
		public Double ListItemHeight
		{
			get => (Double)GetValue(ListItemHeightProperty);
			set => SetValue(ListItemHeightProperty, value);
		}
		#endregion
		#region ListItemBackground
		public static readonly DependencyProperty ListItemBackgroundProperty =
			DependencyProperty.Register(
				nameof(ListItemBackground),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.Gray));
		public Brush ListItemBackground
		{
			get => (Brush)GetValue(ListItemBackgroundProperty);
			set => SetValue(ListItemBackgroundProperty, value);
		}
		#endregion
		#region ListItemMargin
		public static readonly DependencyProperty ListItemMarginProperty =
			DependencyProperty.Register(
				nameof(ListItemMargin),
				typeof(Thickness),
				typeof(AraComboBox),
				new PropertyMetadata(new Thickness(0)));
		public Thickness ListItemMargin
		{
			get => (Thickness)GetValue(ListItemMarginProperty);
			set => SetValue(ListItemMarginProperty, value);
		}
		#endregion
		#region ListItemFontSize
		public static readonly DependencyProperty ListItemFontSizeProperty =
			DependencyProperty.Register(
				nameof(ListItemFontSize),
				typeof(Double),
				typeof(AraComboBox),
				new PropertyMetadata(16.0));
		public Double ListItemFontSize
		{
			get => (Double)GetValue(ListItemFontSizeProperty);
			set => SetValue(ListItemFontSizeProperty, value);
		}
		#endregion
		#region ListItemFontFamily
		public static readonly DependencyProperty ListItemFontFamilyProperty =
			DependencyProperty.Register(
				nameof(ListItemFontFamily),
				typeof(FontFamily),
				typeof(AraComboBox),
				new PropertyMetadata(new FontFamily("Segoe UI")));
		public FontFamily ListItemFontFamily
		{
			get => (FontFamily)GetValue(ListItemFontFamilyProperty);
			set => SetValue(ListItemFontFamilyProperty, value);
		}
		#endregion
		#region ListItemFontWeight
		public static readonly DependencyProperty ListItemFontWeightProperty =
			DependencyProperty.Register(
				nameof(ListItemFontWeight),
				typeof(FontWeight),
				typeof(AraComboBox),
				new PropertyMetadata(FontWeights.Normal));
		public FontWeight ListItemFontWeight
		{
			get => (FontWeight)GetValue(ListItemFontWeightProperty);
			set => SetValue(ListItemFontWeightProperty, value);
		}
		#endregion
		#region ListItemForeground
		public static readonly DependencyProperty ListItemForegroundProperty =
			DependencyProperty.Register(
				nameof(ListItemForeground),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.White));
		public Brush ListItemForeground
		{
			get => (Brush)GetValue(ListItemForegroundProperty);
			set => SetValue(ListItemForegroundProperty, value);
		}
		#endregion
		#region ListItemHoverBackground
		public static readonly DependencyProperty ListItemHoverBackgroundProperty =
			DependencyProperty.Register(
				nameof(ListItemHoverBackground),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.White));
		public Brush ListItemHoverBackground
		{
			get => (Brush)GetValue(ListItemHoverBackgroundProperty);
			set => SetValue(ListItemHoverBackgroundProperty, value);
		}
		#endregion
		#region ListItemSelectedBackground
		public static readonly DependencyProperty ListItemSelectedBackgroundProperty =
			DependencyProperty.Register(
				nameof(ListItemSelectedBackground),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.White));
		public Brush ListItemSelectedBackground
		{
			get => (Brush)GetValue(ListItemSelectedBackgroundProperty);
			set => SetValue(ListItemSelectedBackgroundProperty, value);
		}
		#endregion

		static AraComboBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AraComboBox),
				new FrameworkPropertyMetadata(typeof(AraComboBox)));
		}
	}

}
