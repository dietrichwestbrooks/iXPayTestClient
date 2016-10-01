using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    /// <summary>
    /// Interaction logic for DeviceListView.xaml
    /// </summary>
    [Export(typeof(IDeviceListView))]
    public partial class DeviceListView : IDeviceListView
    {
        [ImportingConstructor]
        public DeviceListView(IDeviceListViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
