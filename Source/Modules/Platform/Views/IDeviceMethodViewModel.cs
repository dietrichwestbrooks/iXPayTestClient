using System.Collections.ObjectModel;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public interface IDeviceMethodViewModel : IDeviceMemberViewModel
    {
        ITerminalDeviceMethod Object { get; }
        ObservableCollection<IDeviceCommandViewModel> Commands { get; }
    }
}
