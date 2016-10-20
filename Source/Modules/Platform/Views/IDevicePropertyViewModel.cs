using System.Collections.ObjectModel;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public interface IDevicePropertyViewModel : IDeviceMemberViewModel
    {
        ITerminalDeviceProperty Object { get; }
        ObservableCollection<IDeviceCommandViewModel> Commands { get; }
    }
}
