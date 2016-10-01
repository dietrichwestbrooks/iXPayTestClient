using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Extensions;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public class MessageBrokerTask
    {
        private readonly TerminalMessage _message;
        private ITerminalClient _terminalClient;
        private ManualResetEventSlim _responseReceived = new ManualResetEventSlim(false);
        private int _sequenceNumber;
        private Exception _exception;
        private object _response;

        public MessageBrokerTask(TerminalMessage message, CancellationToken cancelToken = default(CancellationToken))
        {
            _message = message;

            _terminalClient = ServiceLocator.Current.GetInstance<ITerminalClient>();

            _terminalClient.Error += OnError;
            _terminalClient.ResponseReceived += OnReceived;

            Task = System.Threading.Tasks.Task.Run(() => SendAndWait(), cancelToken);
        }

        public Task<object> Task { get; }

        private void OnReceived(object sender, TerminalMessage message)
        {
            if (message.GetResponseSequenceNumber() != _sequenceNumber)
                return;

            _exception = null;
            _response = message.GetBaseResponse();
            _responseReceived.Set();
        }

        private void OnError(object sender, ClientErrorEventArg args)
        {
            _exception = args.Exception;
            _response = null;
            _responseReceived.Set();
        }

        private object SendAndWait()
        {
            _sequenceNumber = _terminalClient.SendMessage(_message);

            if (!_responseReceived.Wait(10000))
                throw new TimeoutException("Timeout waiting for message response");

            if (_exception != null)
                throw _exception;

            _responseReceived.Reset();

            return _response;
        }
    }
}
