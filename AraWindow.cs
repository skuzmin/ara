using System.Windows;

namespace ARA
{
	public class AraWindow : Window
	{
		static AraWindow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AraWindow),
				new FrameworkPropertyMetadata(typeof(AraWindow)));
		}
	}
}
