using System.Collections.ObjectModel;
using System.Linq;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class DeviceMethodViewModel : ViewModelBase, IDeviceMethodViewModel
    {
        private string _title;

        public DeviceMethodViewModel(ITerminalDeviceMethod method)
        {
            Object = method;
            Title = Object.Name;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<IDeviceCommandViewModel> Commands
            => new ObservableCollection<IDeviceCommandViewModel>(Enumerable.Repeat(new DeviceCommandViewModel(Object.InvokeCommand, DeviceCommandInvokeType.Invoke), 1));

        public ITerminalDeviceMethod Object { get; }
    }
}
