using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    /// <summary>
    /// Interaction logic for ConnectView.xaml
    /// </summary>
    [Export(typeof(IConnectView))]
    public partial class ConnectView : IConnectView
    {
        [ImportingConstructor]
        public ConnectView(IConnectViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        public string FlyoutName { get; } = FlyoutNames.ConnectFlyout;
    }
}
