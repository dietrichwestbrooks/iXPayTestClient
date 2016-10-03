using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class DeviceMethodViewModel : ViewModelBase, ICommandViewModel
    {
        private string _title;
        private CommandType _commandType;
        private ITerminalDeviceMethod _method;

        public DeviceMethodViewModel(ITerminalDeviceMethod method)
        {
            _method = method;
            Title = _method.Name;
            _commandType = CommandType.Method;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        public CommandType CommandType
        {
            get { return _commandType; }
            set { SetProperty(ref _commandType, value); }
        }

        public ITerminalDeviceCommand Object => _method;
    }
}
