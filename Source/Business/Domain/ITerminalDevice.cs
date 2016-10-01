namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public interface ITerminalDevice : ITerminalRequestHandler
    {
        string Name { get; }
        ITerminalDeviceMethodCollection Methods { get; }
        ITerminalDevicePropertyCollection Properties { get; }
    }
}
