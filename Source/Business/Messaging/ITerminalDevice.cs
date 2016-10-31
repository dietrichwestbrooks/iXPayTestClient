using System;
using System.Collections.Generic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDevice : ITerminalRequestHandler, INamedObject
    {
        IEnumerable<ITerminalDeviceMethod> Methods { get; }
        IEnumerable<ITerminalDeviceProperty> Properties { get; }
        IEnumerable<ITerminalDeviceEvent> Events { get; }
        Type CommandType { get; }
        Type ResponseType { get; }
        Type EventType { get; }
        void ClearEventHandlers();
    }
}
