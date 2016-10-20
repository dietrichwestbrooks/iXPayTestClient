using System.Text;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDeviceMember : INamedObject
    {
        ITerminalDevice Device { get; }
    }
}
