using System;
using System.Net;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalConnection : IDisposable
    {
        string HostAddress { get; }
        int HostPort { get; }
        IPEndPoint EndPoint { get; }
        bool IsConnected { get; }
        void Disconnect();
        int SendMessage(TerminalMessage message, MessagePriorty priority = MessagePriorty.Normal);
        event EventHandler<IPEndPoint> Connected;
        event EventHandler<TerminalMessage> MessageSent;
        event EventHandler<IPEndPoint> Disconnected;
        event EventHandler<TerminalMessage> EventReceived;
        event EventHandler<TerminalMessage> ResponseReceived;
        event EventHandler<ConnectionErrorEventArgs> Error;
        event EventHandler<IPEndPoint> Pulse;
    }
}