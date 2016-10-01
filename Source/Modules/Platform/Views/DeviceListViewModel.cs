using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    [Export(typeof(IDeviceListViewModel))]
    public class DeviceListViewModel : ViewModelBase, IDeviceListViewModel
    {
        private ObservableCollection<DeviceViewModel> _devices;

        private string _title = "Devices";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<DeviceViewModel> Devices
        {
            get
            {
                if (_devices == null)
                {
                    _devices =
                        new ObservableCollection<DeviceViewModel>(
                            TerminalClientService.Devices.Select(d => new DeviceViewModel(d)));

                    TerminalClientService.Devices.DeviceAdded += (sender, d) => _devices.Add(new DeviceViewModel(d));
                }

                return _devices;
            }
        }

        [Import]
        private ITerminalClientService TerminalClientService { get; set; }
    }
}
