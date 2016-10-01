namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public interface ITerminalDeviceMethod : ITerminalDeviceCommand
    {
        TerminalMessage GetInvokeMessage(CommandParameters parameters);
        bool ProcessInvokeResponse(object response);
    }
}