using System.Windows.Data;
using System.Windows.Markup;
using ARA.Controls;

namespace ARA.MarkupExtensions
{
	[MarkupExtensionReturnType(typeof(RelativeSource))]
	public class AraComboBoxSourceExtension : MarkupExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new RelativeSource(RelativeSourceMode.FindAncestor, typeof(AraComboBox), 1);
		}
	}
}
