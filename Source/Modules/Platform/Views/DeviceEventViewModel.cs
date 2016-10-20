using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public class DeviceEventViewModel : ViewModelBase, IDeviceEventViewModel
    {
        private string _title;

        public DeviceEventViewModel(ITerminalDeviceEvent @event)
        {
            Object = @event;
            Title = Object.Name;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ITerminalDeviceEvent Object { get; }
    }
}
