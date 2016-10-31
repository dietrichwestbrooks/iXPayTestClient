using System.Net;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public interface IConnectViewModel : IViewModel
    {
        string ServerAddress { get; }
        int ServerPort { get; }
        bool IsConnected { get; }
        bool IsConnecting { get; set; }
        IPEndPoint EndPoint { get; set; }
    }
}
