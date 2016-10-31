using System.Collections.ObjectModel;
using System.Windows.Input;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public interface IDeviceViewModel : IViewModel
    {
        ObservableCollection<IDeviceMemberViewModel> Members { get; }

        ITerminalDevice Object { get; }
    }
}
