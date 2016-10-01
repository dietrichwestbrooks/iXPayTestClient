using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    public interface ITerminalMessageQueue
    {
        bool EnqueueMessage(TerminalMessage message);
        TerminalMessage DequeueMessage();
    }
}
