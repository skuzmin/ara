using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ARA.Animations
{
    public class ColorAnimator
    {
		public static void Animate(Brush toColor, FrameworkElement target, string path)
		{
			if (target == null)
			{
				return;
			}

			var animation = new ColorAnimation
			{
				To = ((SolidColorBrush)toColor).Color,
				Duration = new Duration(TimeSpan.FromMilliseconds(150))
			};

			var storyboard = new Storyboard();
			storyboard.Children.Add(animation);

			Storyboard.SetTarget(animation, target);
			Storyboard.SetTargetProperty(animation, new PropertyPath(path));

			storyboard.Begin();
		}
	}
}
