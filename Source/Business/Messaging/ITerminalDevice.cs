using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDevice : ITerminalRequestHandler, INamedObject
    {
        TerminalDeviceMethodCollection Methods { get; }
        TerminalDevicePropertyCollection Properties { get; }
        TerminalDeviceEventCollection Events { get; }
        Type CommandType { get; }
        Type ResponseType { get; }
        Type EventType { get; }
        void ClearEventHandlers();
    }
}
