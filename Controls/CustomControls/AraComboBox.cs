using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ARA.Animations;
using ARA.Interfaces;

namespace ARA.Controls.CustomControls
{
	class AraComboBox : ComboBox
	{
		private Border? _toggleButtonBorder;
		private TextBox? _editableTextBox;
		private string _lastText = string.Empty;

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
		#region ItemsListBorderThickness
		public static readonly DependencyProperty ItemsListBorderThicknessProperty =
			DependencyProperty.Register(
				nameof(ItemsListBorderThickness),
				typeof(Thickness),
				typeof(AraComboBox),
				new PropertyMetadata(new Thickness(1)));
		public Thickness ItemsListBorderThickness
		{
			get => (Thickness)GetValue(ItemsListBorderThicknessProperty);
			set => SetValue(ItemsListBorderThicknessProperty, value);
		}
		#endregion
		#region ItemsListBorderBrush
		public static readonly DependencyProperty ItemsListBorderBrushProperty =
			DependencyProperty.Register(
				nameof(ItemsListBorderBrush),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.Black));
		public Brush ItemsListBorderBrush
		{
			get => (Brush)GetValue(ItemsListBorderBrushProperty);
			set => SetValue(ItemsListBorderBrushProperty, value);
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
				new PropertyMetadata(Brushes.Transparent));
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
				new PropertyMetadata(Brushes.Green));
		public Brush ListItemSelectedBackground
		{
			get => (Brush)GetValue(ListItemSelectedBackgroundProperty);
			set => SetValue(ListItemSelectedBackgroundProperty, value);
		}
		#endregion

		#region ToggleButtonBackground
		public static readonly DependencyProperty ToggleButtonBackgroundProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonBackground),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.Transparent));
		public Brush ToggleButtonBackground
		{
			get => (Brush)GetValue(ToggleButtonBackgroundProperty);
			set => SetValue(ToggleButtonBackgroundProperty, value);
		}
		#endregion
		#region ToggleButtonBorderThickness
		public static readonly DependencyProperty ToggleButtonBorderThicknessProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonBorderThickness),
				typeof(Thickness),
				typeof(AraComboBox),
				new PropertyMetadata(new Thickness(1)));
		public Thickness ToggleButtonBorderThickness
		{
			get => (Thickness)GetValue(ToggleButtonBorderThicknessProperty);
			set => SetValue(ToggleButtonBorderThicknessProperty, value);
		}
		#endregion
		#region ToggleButtonCornerRadius
		public static readonly DependencyProperty ToggleButtonCornerRadiusProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonCornerRadius),
				typeof(CornerRadius),
				typeof(AraComboBox),
				new PropertyMetadata(new CornerRadius(0)));
		public CornerRadius ToggleButtonCornerRadius
		{
			get => (CornerRadius)GetValue(ToggleButtonCornerRadiusProperty);
			set => SetValue(ToggleButtonCornerRadiusProperty, value);
		}
		#endregion
		#region ToggleButtonBorderBrush
		public static readonly DependencyProperty ToggleButtonBorderBrushProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonBorderBrush),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.Black));
		public Brush ToggleButtonBorderBrush
		{
			get => (Brush)GetValue(ToggleButtonBorderBrushProperty);
			set => SetValue(ToggleButtonBorderBrushProperty, value);
		}
		#endregion
		#region ToggleButtonArrowHeight
		public static readonly DependencyProperty ToggleButtonArrowHeightProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonArrowHeight),
				typeof(Double),
				typeof(AraComboBox),
				new PropertyMetadata(16.0));
		public Double ToggleButtonArrowHeight
		{
			get => (Double)GetValue(ToggleButtonArrowHeightProperty);
			set => SetValue(ToggleButtonArrowHeightProperty, value);
		}
		#endregion
		#region ToggleButtonArrowWidth
		public static readonly DependencyProperty ToggleButtonArrowWidthProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonArrowWidth),
				typeof(Double),
				typeof(AraComboBox),
				new PropertyMetadata(16.0));
		public Double ToggleButtonArrowWidth
		{
			get => (Double)GetValue(ToggleButtonArrowWidthProperty);
			set => SetValue(ToggleButtonArrowWidthProperty, value);
		}
		#endregion
		#region ToggleButtonArrowMargin
		public static readonly DependencyProperty ToggleButtonArrowMarginProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonArrowMargin),
				typeof(Thickness),
				typeof(AraComboBox),
				new PropertyMetadata(new Thickness(0)));
		public Thickness ToggleButtonArrowMargin
		{
			get => (Thickness)GetValue(ToggleButtonArrowMarginProperty);
			set => SetValue(ToggleButtonArrowMarginProperty, value);
		}
		#endregion
		#region ToggleButtonArrowBrush
		public static readonly DependencyProperty ToggleButtonArrowBrushProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonArrowBrush),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.Black));
		public Brush ToggleButtonArrowBrush
		{
			get => (Brush)GetValue(ToggleButtonArrowBrushProperty);
			set => SetValue(ToggleButtonArrowBrushProperty, value);
		}
		#endregion
		#region ToggleButtonHoverBorderBrush
		public static readonly DependencyProperty ToggleButtonHoverBorderBrushProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonHoverBorderBrush),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.Gray));
		public Brush ToggleButtonHoverBorderBrush
		{
			get => (Brush)GetValue(ToggleButtonHoverBorderBrushProperty);
			set => SetValue(ToggleButtonHoverBorderBrushProperty, value);
		}
		#endregion
		#region ToggleButtonHoverBackground
		public static readonly DependencyProperty ToggleButtonHoverBackgroundProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonHoverBackground),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.Transparent));
		public Brush ToggleButtonHoverBackground
		{
			get => (Brush)GetValue(ToggleButtonHoverBackgroundProperty);
			set => SetValue(ToggleButtonHoverBackgroundProperty, value);
		}
		#endregion
		#region ToggleButtonHoverArrowBrush
		public static readonly DependencyProperty ToggleButtonHoverArrowBrushProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonHoverArrowBrush),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.Gray));
		public Brush ToggleButtonHoverArrowBrush
		{
			get => (Brush)GetValue(ToggleButtonHoverArrowBrushProperty);
			set => SetValue(ToggleButtonHoverArrowBrushProperty, value);
		}
		#endregion
		#region ToggleButtonOpenBorderBrush
		public static readonly DependencyProperty ToggleButtonOpenBorderBrushProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonOpenBorderBrush),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.White));
		public Brush ToggleButtonOpenBorderBrush
		{
			get => (Brush)GetValue(ToggleButtonOpenBorderBrushProperty);
			set => SetValue(ToggleButtonOpenBorderBrushProperty, value);
		}
		#endregion
		#region ToggleButtonOpenBackground
		public static readonly DependencyProperty ToggleButtonOpenBackgroundProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonOpenBackground),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.Transparent));
		public Brush ToggleButtonOpenBackground
		{
			get => (Brush)GetValue(ToggleButtonOpenBackgroundProperty);
			set => SetValue(ToggleButtonOpenBackgroundProperty, value);
		}
		#endregion
		#region ToggleButtonOpenArrowBrush
		public static readonly DependencyProperty ToggleButtonOpenArrowBrushProperty =
			DependencyProperty.Register(
				nameof(ToggleButtonOpenArrowBrush),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.White));
		public Brush ToggleButtonOpenArrowBrush
		{
			get => (Brush)GetValue(ToggleButtonOpenArrowBrushProperty);
			set => SetValue(ToggleButtonOpenArrowBrushProperty, value);
		}
		#endregion

		#region PlaceholderText
		public static readonly DependencyProperty PlaceholderTextProperty =
			DependencyProperty.Register(
				nameof(PlaceholderText),
				typeof(string),
				typeof(AraComboBox),
				new PropertyMetadata(string.Empty));

		public string PlaceholderText
		{
			get { return (string)GetValue(PlaceholderTextProperty); }
			set { SetValue(PlaceholderTextProperty, value); }
		}
		#endregion
		#region PlaceholderForeground
		public static readonly DependencyProperty PlaceholderForegroundProperty =
			DependencyProperty.Register(
				nameof(PlaceholderForeground),
				typeof(Brush),
				typeof(AraComboBox),
				new PropertyMetadata(Brushes.Gray));

		public Brush PlaceholderForeground
		{
			get { return (Brush)GetValue(PlaceholderForegroundProperty); }
			set { SetValue(PlaceholderForegroundProperty, value); }
		}
		#endregion

		#region TextBoxMargin
		public static readonly DependencyProperty TextBoxMarginProperty =
			DependencyProperty.Register(
				nameof(TextBoxMargin),
				typeof(Thickness),
				typeof(AraComboBox),
				new PropertyMetadata(new Thickness(0)));
		public Thickness TextBoxMargin
		{
			get => (Thickness)GetValue(TextBoxMarginProperty);
			set => SetValue(TextBoxMarginProperty, value);
		}
		#endregion

		#region Init
		static AraComboBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AraComboBox),
				new FrameworkPropertyMetadata(typeof(AraComboBox)));
			IsTextSearchEnabledProperty.OverrideMetadata(typeof(AraComboBox),
				new FrameworkPropertyMetadata(false));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			InitToggleButtonBorder();
			DetachTextBoxEvents();
			_editableTextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
			AttachTextBoxEvents();
		}
		private void InitToggleButtonBorder()
		{
			if (GetTemplateChild("PART_ToggleButton") is ToggleButton toggleBtn)
			{
				toggleBtn.ApplyTemplate();
				_toggleButtonBorder = toggleBtn.Template.FindName("ToggleBorder", toggleBtn) as Border;
				if (_toggleButtonBorder != null)
				{
					_toggleButtonBorder.Background = ToggleButtonBackground;
					_toggleButtonBorder.BorderBrush = ToggleButtonBorderBrush;
				}
			}
		}
		private void DetachTextBoxEvents()
		{
			if (_editableTextBox == null)
			{
				return;
			}

			_editableTextBox.PreviewMouseLeftButtonDown -= OnTextBoxClicked;
			_editableTextBox.GotKeyboardFocus -= OnGotKeyboardFocus;
			_editableTextBox.LostKeyboardFocus -= OnLostKeyboardFocus;
			_editableTextBox.PreviewTextInput -= OnTextBoxTextChanged;
			_editableTextBox.PreviewKeyUp -= OnTextBoxKeyDown;
			_editableTextBox.SelectionChanged -= OnTextBoxSelectionChanged;
		}
		private void AttachTextBoxEvents()
		{
			if (_editableTextBox == null)
			{
				return;
			}

			_editableTextBox.PreviewMouseLeftButtonDown += OnTextBoxClicked;
			_editableTextBox.GotKeyboardFocus += OnGotKeyboardFocus;
			_editableTextBox.LostKeyboardFocus += OnLostKeyboardFocus;
			_editableTextBox.PreviewTextInput += OnTextBoxTextChanged;
			_editableTextBox.PreviewKeyDown += OnTextBoxKeyDown;
			_editableTextBox.SelectionChanged += OnTextBoxSelectionChanged;
		}
		#endregion

		#region AutoComplete
		public void Reset()
		{
			Text = "";
			SelectedItem = null;
			Focus();
		}
		private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Space:
				case Key.Back:
				case Key.Delete:
					CleanSelectedItem();
					break;
			}
		}
		private void OnTextBoxTextChanged(object sender, TextCompositionEventArgs e)
		{
			if (e.Text == "\r" || e.Text == "\n")
			{
				return;
			}
			CleanSelectedItem();
		}
		private void ApplyFilter(string filterText)
		{
			ICollectionView view = CollectionViewSource.GetDefaultView(ItemsSource);
			if (view == null)
			{
				return;
			}

			if (string.IsNullOrEmpty(filterText))
			{
				view.Filter = null;
			}
			else
			{
				view?.Filter = item => item is IFilterable i && i.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase);
			}

			IsDropDownOpen = view!.Cast<object>().Any();
		}
		private void CleanSelectedItem()
		{
			if (SelectedItem != null)
			{
				int caret = _editableTextBox!.CaretIndex;
				SelectedItem = null;
				_editableTextBox.CaretIndex = caret;
				_editableTextBox.SelectionLength = 0;
			}
			Dispatcher.InvokeAsync(() =>
			{

				var currentText = _editableTextBox?.Text ?? string.Empty;
				if (currentText == _lastText)
				{
					return;
				}

				ApplyFilter(currentText);

				if (SelectedItem != null && _editableTextBox != null)
				{
					int caretPosition = _editableTextBox.CaretIndex;
					SelectedItem = null;
					_editableTextBox.CaretIndex = caretPosition;
				}
				_lastText = currentText;
			});
		}
		private void OnTextBoxSelectionChanged(object sender, RoutedEventArgs e)
		{
			if (_editableTextBox!.SelectionLength > 0)
			{
				_editableTextBox.SelectionLength = 0;
				_editableTextBox.CaretIndex = _editableTextBox.Text.Length;
			}
		}
		private void OnMouseDownOutsideDropdown(object sender, MouseButtonEventArgs e)
		{
			IsDropDownOpen = false;
		}
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			string currentText = Text;
			base.OnSelectionChanged(e);
			if (IsEditable && string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(currentText))
			{
				Text = currentText;
			}
		}
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (e.Key != Key.Up && e.Key != Key.Down)
			{
				base.OnPreviewKeyDown(e);
				return;
			}

			if (!IsDropDownOpen)
			{
				IsDropDownOpen = true;
				e.Handled = true;
				return;
			}

			if (IsEditable && SelectedItem != null)
			{
				base.OnPreviewKeyDown(e);
				return;
			}

			if (Items.Count > 0)
			{
				SelectedIndex = e.Key == Key.Down
					? Math.Min(SelectedIndex + 1, Items.Count - 1)
					: Math.Max(SelectedIndex - 1, 0);
			}
			e.Handled = true;
		}
		protected override void OnDropDownOpened(EventArgs e)
		{
			base.OnDropDownOpened(e);
			Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideDropdown);
		}
		protected override void OnDropDownClosed(EventArgs e)
		{
			base.OnDropDownClosed(e);
			Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideDropdown);
		}
		#endregion

		#region Animations
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			if (_editableTextBox != null && !_editableTextBox.IsKeyboardFocused)
			{
				ColorAnimator.Animate(ToggleButtonHoverBackground, _toggleButtonBorder!, "(Background).(SolidColorBrush.Color)");
				ColorAnimator.Animate(ToggleButtonHoverBorderBrush, _toggleButtonBorder!, "(BorderBrush).(SolidColorBrush.Color)");
			}
		}
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			if (_editableTextBox != null && !_editableTextBox.IsKeyboardFocused)
			{
				ColorAnimator.Animate(ToggleButtonBackground, _toggleButtonBorder!, "(Background).(SolidColorBrush.Color)");
				ColorAnimator.Animate(ToggleButtonBorderBrush, _toggleButtonBorder!, "(BorderBrush).(SolidColorBrush.Color)");
			}
		}
		private void OnTextBoxClicked(object sender, MouseButtonEventArgs e)
		{
			if (_editableTextBox != null && _editableTextBox.IsKeyboardFocused)
			{
				IsDropDownOpen = true;
			}
		}
		private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (_editableTextBox != null && _editableTextBox.IsKeyboardFocused)
			{
				IsDropDownOpen = true;
			}
			ColorAnimator.Animate(ToggleButtonHoverBackground, _toggleButtonBorder!, "(Background).(SolidColorBrush.Color)");
			ColorAnimator.Animate(ToggleButtonHoverBorderBrush, _toggleButtonBorder!, "(BorderBrush).(SolidColorBrush.Color)");

		}
		private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (_editableTextBox != null && !IsKeyboardFocusWithin)
			{
				IsDropDownOpen = false;
			}
			ColorAnimator.Animate(ToggleButtonBackground, _toggleButtonBorder!, "(Background).(SolidColorBrush.Color)");
			ColorAnimator.Animate(ToggleButtonBorderBrush, _toggleButtonBorder!, "(BorderBrush).(SolidColorBrush.Color)");
		}
		#endregion
	}
}
