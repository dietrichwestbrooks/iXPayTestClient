namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public interface ITerminalDeviceMethod : ITerminalDeviceCommand
    {
        TerminalMessage GetMessage();
        TerminalMessage GetMessage(CommandParameters parameters);
        bool ProcessInvokeResponse(object response);
    }
}