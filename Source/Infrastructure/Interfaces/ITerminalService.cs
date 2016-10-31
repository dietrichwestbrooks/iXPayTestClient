using System.Net;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces
{
    public interface ITerminalService
    {
        TerminalDeviceCollection Devices { get; }
        void RegisterDevice(ITerminalDevice device);
        ITerminalDevice RegisterDeviceFromFile(string path);
        void Disconnect(IPEndPoint endPoint);
        void SendMessage(TerminalMessage message);
        void Connect(IPEndPoint endPoint, bool isClient, bool autoReConnect, bool isSecure);
    }
}
