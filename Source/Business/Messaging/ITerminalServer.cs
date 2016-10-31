using System.Net;
using System.Threading.Tasks;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    internal interface ITerminalServer : ITerminalConnection
    {
        Task StartAsync(IPEndPoint endPoint, bool isSecure);
    }
}