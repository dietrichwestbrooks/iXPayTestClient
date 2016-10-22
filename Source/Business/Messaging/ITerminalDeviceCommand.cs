using System;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDeviceCommand
    {
        string Name { get; }
        Type CommandType { get; }
        Type ResponseType { get; }
        TerminalMessage GetMessage(CommandParameters parameters = null);
        object Execute(CommandParameters parameters);
        bool Result { get; }
        string ResultMessage { get; }
        ITerminalDeviceMember Member { get; }
    }
}
