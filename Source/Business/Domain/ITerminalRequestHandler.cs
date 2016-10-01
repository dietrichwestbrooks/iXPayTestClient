using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public interface ITerminalRequestHandler
    {
        ITerminalRequestHandler Successor { get; }
        object HandleRequest(object command);
    }
}
