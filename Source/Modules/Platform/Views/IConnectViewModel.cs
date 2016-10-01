using System.Net;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public interface IConnectViewModel : IViewModel
    {
        string HostAddress { get; }
        int HostPort { get; }
        bool IsConnected { get; }
        bool IsConnecting { get; set; }
        IPEndPoint EndPoint { get; set; }
    }
}
