using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Desktop.Views
{
    public interface IShellViewModel : IViewModel
    {
        bool IsHeartBeating { get; set; }
    }
}
