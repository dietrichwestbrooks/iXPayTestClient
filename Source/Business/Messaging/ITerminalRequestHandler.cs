namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalRequestHandler
    {
        ITerminalRequestHandler Successor { get; }
        object HandleRequest(object command);
    }
}
