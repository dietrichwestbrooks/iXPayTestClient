namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDeviceMethod : ITerminalDeviceCommand
    {
        TerminalMessage GetInvokeMessage(CommandParameters parameters);
        bool ProcessInvokeResponse(object response);
    }
}