using System.Net;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces
{
    public interface ITerminalClientService
    {
        TerminalDeviceCollection Devices { get; }
        bool IsConnected { get; }
        void RegisterDevice(ITerminalDevice device);
        ITerminalDevice RegisterDeviceFromFile(string path);
        void Connect(IPEndPoint endPoint);
        void Disconnect(IPEndPoint endPoint);
        void SendMessage(TerminalMessage message);
    }
}
