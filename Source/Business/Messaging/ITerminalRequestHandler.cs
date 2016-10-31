namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalRequestHandler
    {
        ITerminalRequestHandler Successor { get; set; }
        object HandleRequest(object command);
    }
}
