using System.Net;
using System.Threading.Tasks;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    internal interface ITerminalClient : ITerminalConnection
    {
        Task ConnectAsync(IPEndPoint endPoint, bool isSecure, bool keepAlive);
    }
}
