using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ARA.Enums;

namespace ARA.Controls.CustomControls
{
	public partial class AraTextBox : TextBox
	{
		#region CornerRadius
		public static readonly DependencyProperty CornerRadiusProperty =
			DependencyProperty.Register(
				nameof(CornerRadius),
				typeof(CornerRadius),
				typeof(AraTextBox),
				new PropertyMetadata(new CornerRadius(0)));
		public CornerRadius CornerRadius
		{
			get => (CornerRadius)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}
		#endregion
		#region InputType
		public static readonly DependencyProperty InputTypeProperty =
			DependencyProperty.Register(
				nameof(InputType),
				typeof(TextBoxInputType),
				typeof(AraTextBox),
				new PropertyMetadata(TextBoxInputType.Text, OnInputTypeChanged));
		public TextBoxInputType InputType
		{
			get => (TextBoxInputType)GetValue(InputTypeProperty);
			set => SetValue(InputTypeProperty, value);
		}
		#endregion
		static AraTextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AraTextBox),
				new FrameworkPropertyMetadata(typeof(AraTextBox)));
		}
		#region TypeValidation
		private static void OnInputTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var textBox = (AraTextBox)d;
			textBox?.UpdateValidation();
		}
		private void UpdateValidation()
		{
			PreviewTextInput -= NumericValidation;
			DataObject.RemovePastingHandler(this, OnPaste);
			if (InputType == TextBoxInputType.Number)
			{
				PreviewTextInput += NumericValidation;
				DataObject.AddPastingHandler(this, OnPaste);
			}
		}
		private void NumericValidation(object sender, TextCompositionEventArgs e)
		{
			Regex regex = NumericRegex();
			e.Handled = regex.IsMatch(e.Text);
		}
		private void OnPaste(object sender, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(typeof(string)))
			{
				var text = (string)e.DataObject.GetData(typeof(string));
				Regex regex = NumericRegex();
				if (regex.IsMatch(text))
				{
					e.CancelCommand();
				}
			}
			else
			{
				e.CancelCommand();
			}
		}

		[GeneratedRegex("[^0-9]+")]
		private static partial Regex NumericRegex();
		#endregion
	}
}
