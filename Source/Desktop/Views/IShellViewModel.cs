using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Desktop.Views
{
    public interface IShellViewModel : IViewModel
    {
        bool IsHeartBeating { get; }
        bool IsConnected { get; }
        string HostAddress { get; }
    }
}
