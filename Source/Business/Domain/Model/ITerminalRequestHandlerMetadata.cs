using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    public interface ITerminalRequestHandlerMetadata
    {
        Type Command { get; }

        Type Response { get; }

        Type Successor { get; }
    }
}