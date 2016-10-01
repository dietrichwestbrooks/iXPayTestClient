namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    public interface IDispenser
    {
        IPumpController Pump { get; }
        ITerminalController Terminal { get; }
    }
}
