using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public interface ITerminalDeviceCommand
    {
        string Name { get; }
        ITerminalRequestHandler Successor { get; }
        bool Result { get; }
        string ResultMessage { get; }
        bool ProcessResponse(object response);
    }
}
