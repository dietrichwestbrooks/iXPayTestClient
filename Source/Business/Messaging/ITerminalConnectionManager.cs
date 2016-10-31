using System;
using System.Net;
using System.Threading.Tasks;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalConnectionManager : IDisposable
    {
        ITerminalConnection Connection { get; }
        event EventHandler<ActiveConnectionChangedArgs> ConnectionChanged;
        Task ConnectAsync(IPEndPoint endPoint, bool isSecure, bool keepAlive);
        Task StartAsync(IPEndPoint endPoint, bool isSecure);
    }
}
