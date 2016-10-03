namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDeviceProperty : ITerminalDeviceCommand
    {
        TerminalMessage GetGetMessage(CommandParameters parameters);
        TerminalMessage GetSetMessage(CommandParameters parameters);
        object Value { get; set; }
        bool ProcessGetResponse(object response);
        bool ProcessSetResponse(object response);
    }
}