using System.Windows;
using System.Windows.Controls;
using ARA.Enums;

namespace ARA.Controls
{
    public partial class LoadoutStatus : UserControl
    {
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register(
                nameof(Status),
                typeof(GameItemStatus),
                typeof(LoadoutStatus)
            );

        public GameItemStatus Status
        {
            get => (GameItemStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public LoadoutStatus()
        {
            InitializeComponent();
        }
    }
}
