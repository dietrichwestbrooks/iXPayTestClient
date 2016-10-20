using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class DeviceCommandViewModel : ViewModelBase, IDeviceCommandViewModel
    {
        private DeviceCommandInvokeType _invokeType;
        private string _title;

        public DeviceCommandViewModel(ITerminalDeviceCommand command, DeviceCommandInvokeType invokeType)
        {
            Object = command;

            InvokeType = invokeType;

            Title = Object.CommandType.Name;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DeviceCommandInvokeType InvokeType
        {
            get { return _invokeType; }
            set { SetProperty(ref _invokeType, value); }
        }

        public ITerminalDeviceCommand Object { get; }
    }
}
