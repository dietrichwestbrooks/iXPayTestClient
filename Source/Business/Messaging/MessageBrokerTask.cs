using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    internal class MessageBrokerTask
    {
        private ManualResetEventSlim _responseReceived = new ManualResetEventSlim(false);
        private int _sequenceNumber;
        private object _response;

        public MessageBrokerTask(TerminalMessage message, CancellationToken cancelToken = default(CancellationToken))
        {
            var connectionManager = ServiceLocator.Current.GetInstance<ITerminalConnectionManager>();
            var connection = connectionManager.Connection;

            if (connection == null)
                throw new InvalidOperationException("No connection");

            connection.ResponseReceived += OnReceived;

            Task = System.Threading.Tasks.Task.Run(() => Send(connection, message), cancelToken);
        }

        public Task<object> Task { get; }

        private void OnReceived(object sender, TerminalMessage message)
        {
            if (message.GetResponseSequenceNumber() != _sequenceNumber)
                return;

            _response = message.GetLastItem();
            _responseReceived.Set();
        }

        private object Send(ITerminalConnection connection, TerminalMessage message)
        {
            _sequenceNumber = connection.SendMessage(message);

            if (!_responseReceived.Wait(30000))
                throw new TimeoutException("Timeout waiting for message response");

            return _response;
        }
    }
}
