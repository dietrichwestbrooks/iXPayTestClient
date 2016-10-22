using System;
using System.Net;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalClient : IDisposable
    {
        string HostAddress { get; }
        int HostPort { get; }
        IPEndPoint EndPoint { get; }
        bool IsConnected { get; }
        bool Connect(IPEndPoint endPoint);
        bool Connect(string address, int port);
        Task<bool> ConnectAsync(IPEndPoint endPoint);
        Task<bool> ConnectAsync(string address, int port);
        void Disconnect();
        int SendMessage(TerminalMessage message, MessagePriorty priority = MessagePriorty.Normal);
        event EventHandler<IPEndPoint> Connected;
        event EventHandler<TerminalMessage> MessageSent;
        event EventHandler<IPEndPoint> Disconnected;
        event EventHandler<TerminalMessage> EventReceived;
        event EventHandler<TerminalMessage> ResponseReceived;
        event EventHandler<ClientErrorEventArg> Error;
        event EventHandler<PulseEventArgs> Pulse;
    }
}