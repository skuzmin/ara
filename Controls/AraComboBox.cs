using System.Windows;
using System.Windows.Controls;

namespace ARA.Controls
{
	class AraComboBox : ComboBox
	{
		static AraComboBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AraComboBox),
				new FrameworkPropertyMetadata(typeof(AraComboBox)));
		}
	}

}
