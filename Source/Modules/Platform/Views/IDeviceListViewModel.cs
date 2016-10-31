using System.Windows.Input;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public interface IDeviceListViewModel : IViewModel
    {
        ICommand SelectedObjectChangedCommand { get; }
    }
}
