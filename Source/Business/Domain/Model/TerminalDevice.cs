namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    public abstract class TerminalDevice : ITerminalDevice
    {
        public virtual bool IsActive => false;
    }
}
