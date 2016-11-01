namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDeviceProperty : ITerminalDeviceMember
    {
        ITerminalDeviceCommand GetCommand { get; }
        ITerminalDeviceCommand SetCommand { get; }
        PropertyInvoke InvokeFlag { get; set; }
        string ValuePropertyName { get; }
        bool TryGet(CommandParameters parameters, out object result);
        bool TrySet(CommandParameters parameters);
    }
}