using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    internal abstract class TerminalConnection : ITerminalConnection
    {
        private bool _disposed;

        ProducerConsumerQueue _outboundMessageQueue;
        ProducerConsumerQueue _inboundMessageQueue;

        private static int _sequenceNumber;
        private static object _sequenceLocker = new object();

        protected TerminalConnection()
        {
            _outboundMessageQueue = new ProducerConsumerQueue(2);
            _inboundMessageQueue = new ProducerConsumerQueue(2);
        }

        ~TerminalConnection()
        {
            Dispose(false);
        }

        public event EventHandler<ConnectionErrorEventArgs> Error;
        public event EventHandler<IPEndPoint> Connected;
        public event EventHandler<IPEndPoint> Disconnected;
        public event EventHandler<IPEndPoint> Pulse;
        public event EventHandler<TerminalMessage> MessageSent;
        public event EventHandler<TerminalMessage> EventReceived;
        public event EventHandler<TerminalMessage> ResponseReceived;

        public IPEndPoint EndPoint { get; protected set; }

        public string HostAddress => EndPoint.Address.ToString();

        public int HostPort => EndPoint.Port;

        protected CancellationTokenSource ShutdownToken { get; private set; }

        protected TcpClient Client { get; private set; }

        public bool IsConnected => Client?.Connected ?? false;

        protected void Connect(TcpClient client)
        {
            using (new TerminalConnectionLocker())
            {
                Client = client;

                ShutdownToken = new CancellationTokenSource();

                Task.Factory.StartNew(InboundMessageLoop);
            }

            FireConnected();

            OnConnected();
        }

        protected virtual void OnConnected()
        {
        }

        public void Disconnect()
        {
            using (new TerminalConnectionLocker())
            {
                ShutdownToken.Cancel();

                if (Client.Connected)
                    Client.Close();

                EndPoint = null;
            }

            FireDisconnected();

            OnDisconnected();
        }

        protected virtual void OnDisconnected()
        {
        }

        public int SendMessage(TerminalMessage message, MessagePriorty priority = MessagePriorty.Normal)
        {
            int sequenceNumber;

            using (new TerminalConnectionLocker())
            {
                if (Client == null || !Client.Connected)
                    throw new InvalidOperationException("No Connection");

                sequenceNumber = IncSequenceNumber;

                message.SetCommandSequenceNumber(sequenceNumber);

                _outboundMessageQueue.Enqueue(() => Send(message), ShutdownToken.Token); 
            }

            return sequenceNumber;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Disconnect();
                _inboundMessageQueue.Dispose();
                _outboundMessageQueue.Dispose();
            }

            _disposed = true;
        }

        private void InboundMessageLoop()
        {
            try
            {
                while (!ShutdownToken.Token.WaitHandle.WaitOne(100))
                {
                    using (new TerminalConnectionLocker())
                    {
                        NetworkStream reader = Client.GetStream();

                        if (!reader.DataAvailable)
                            continue;

                        byte[] bLength = new byte[4];

                        reader.Read(bLength, 0, bLength.Length);

                        int length = BitConverter.ToInt32(bLength, 0);

                        length = IPAddress.NetworkToHostOrder(length);

                        string xmlMessage = ReadBuffer(length);

                        _inboundMessageQueue.Enqueue(() => Receive(xmlMessage), ShutdownToken.Token);
                    }

                }
            }
            catch (Exception ex)
            {
                FireError(ClientErrorType.DataReceiveError, ex);
            }
        }

        protected virtual void Receive(string xmlMessage)
        {
            var serializer = ServiceLocator.Current.GetInstance<ITerminalMessageSerializer>();
            var message = serializer.Deserialize(xmlMessage);

            var @event = message.Item as TerminalEvent;
            if (@event != null)
            {
                FireEventReceived(message);
                return;
            }

            var response = message.Item as TerminalResponse;
            if (response != null)
                FireResponseReceived(message);
        }

        protected virtual void Send(TerminalMessage message)
        {
            try
            {
                byte[] buffer = message.GetBytes();

                var length = IPAddress.HostToNetworkOrder(buffer.Length);

                using (new TerminalConnectionLocker())
                {
                    if (Client != null && Client.Connected)
                        Client.Client.Send(BitConverter.GetBytes(length).Concat(buffer).ToArray());
                } 

                FireMessageSent(message);
            }
            catch (Exception ex)
            {
                FireError(ClientErrorType.DataSendError, ex);
            }
        }

        private static int IncSequenceNumber
        {
            get
            {
                lock (_sequenceLocker) return ++_sequenceNumber;
            }
        }

        private string ReadBuffer(int length)
        {
            var reader = Client.GetStream();

            byte[] buffer = new byte[1024];
            string data = string.Empty;
            int bytesNeeded = length;
            int bytesReceived = 0;

            do
            {
                Array.Clear(buffer, 0, buffer.Length);
                int bytesRead = reader.Read(buffer, 0, bytesNeeded - bytesReceived >= buffer.Length ? buffer.Length : bytesNeeded - bytesReceived);
                bytesReceived += bytesRead;
                data += Encoding.UTF8.GetString(buffer);
            } while (bytesReceived < bytesNeeded);

            return data.Trim().TrimEnd('\0');
        }

        protected void FireMessageSent(TerminalMessage message)
        {
            MessageSent?.Invoke(this, message);
        }

        protected void FireConnected()
        {
            Connected?.Invoke(this, EndPoint);
        }

        protected void FireDisconnected()
        {
            Disconnected?.Invoke(this, EndPoint);
        }

        protected void FireEventReceived(TerminalMessage message)
        {
            EventReceived?.Invoke(this, message);
        }

        protected void FireResponseReceived(TerminalMessage message)
        {
            ResponseReceived?.Invoke(this, message);
        }

        protected void FireError(ClientErrorType type, Exception ex)
        {
            Error?.Invoke(this, new ConnectionErrorEventArgs {Exception = ex, EndPoint = EndPoint, ErrorType = type, IsError = true});
        }

        protected void FirePulse()
        {
            Pulse?.Invoke(this, EndPoint);
        }
    }
}
