using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views
{
    /// <summary>
    /// Interaction logic for NotificationView.xaml
    /// </summary>
    [Export(typeof(INotificationView))]
    public partial class NotificationView : INotificationView
    {
        [ImportingConstructor]
        public NotificationView(INotificationViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        public string FlyoutName { get; } = FlyoutNames.NotificationFlyout;
    }
}
