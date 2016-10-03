using System.Net;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces
{
    public interface ITerminalClientService
    {
        ITerminalDeviceCollection Devices { get; }
        bool IsConnected { get; }
        void RegisterDevice(ITerminalDevice device);
        ITerminalDevice RegisterDeviceFromFile(string path);
        void Connect(IPEndPoint endPoint);
        void Disconnect(IPEndPoint endPoint);
    }
}
