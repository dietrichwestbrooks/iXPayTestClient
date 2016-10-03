namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDevice : ITerminalRequestHandler
    {
        string Name { get; }
        ITerminalDeviceMethodCollection Methods { get; }
        ITerminalDevicePropertyCollection Properties { get; }
    }
}
