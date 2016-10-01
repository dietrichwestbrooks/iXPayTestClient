using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class DevicePropertyViewModel : ViewModelBase, ICommandViewModel
    {
        private string _title;
        private CommandType _commandType;
        private ITerminalDeviceProperty _property;

        public DevicePropertyViewModel(ITerminalDeviceProperty property)
        {
            _property = property;
            Title = _property.Name;
            _commandType = CommandType.Property;
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

        public ITerminalDeviceCommand Object => _property;
    }
}
