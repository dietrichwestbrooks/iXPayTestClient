namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDeviceMethod : ITerminalDeviceMember
    {
        ITerminalDeviceCommand InvokeCommand { get; }
        bool TryInvoke(CommandParameters parameters, out object result);
    }
}