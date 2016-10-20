using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public interface IDeviceEventViewModel : IDeviceMemberViewModel
    {
        ITerminalDeviceEvent Object { get; }
    }
}
