using System.Collections.Generic;
using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Services
{
    [Export(typeof(ITerminalMessageQueue))]
    [Export(typeof(ITerminalMessageRouterService))]
    public class TerminalMessageRouterService : ITerminalMessageRouterService, ITerminalMessageQueue
    {
        private Queue<TerminalMessage> _messageQueue = new Queue<TerminalMessage>();

        public bool EnqueueMessage(TerminalMessage message)
        {
            _messageQueue.Enqueue(message);
            return true;
        }

        public TerminalMessage DequeueMessage()
        {
            return _messageQueue.Dequeue();
        }

        public void ClearMessages()
        {
            _messageQueue.Clear();
        }

        public void PumpMessage(int numToSend, bool auto)
        {
        }

        public void PumpAllMessage(bool auto)
        {
        }

        public void SendMessage(TerminalMessage message)
        {
        }

        public TerminalMessage PeekMessage(bool remove = false)
        {
            return remove ? DequeueMessage() : _messageQueue.Peek();
        }

        public void Suspend()
        {
        }

        public void Resume()
        {
        }
    }
}
