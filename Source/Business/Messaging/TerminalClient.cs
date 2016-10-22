using System;
using System.ComponentModel.Composition;
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
    [Export(typeof(ITerminalClient))]
    public class TerminalClient : ITerminalClient
    {
        private bool _disposed;
        private TcpClient _client;
        private Timer _pulseTimer;

        ProducerConsumerQueue _outboundMessageQueue;

        ProducerConsumerQueue _inboundMessageQueue;
        private Task _inboundLoopTask;
        private int _inboundLoopSleepTime = 500;

        CancellationTokenSource _shutdownToken;

        private static int _sequenceNumber;
        private static object _sequenceLocker = new object();

        public TerminalClient()
        {
            _pulseTimer = new Timer(OnPulseTimerElapsed, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

            _outboundMessageQueue = new ProducerConsumerQueue(2);
            _inboundMessageQueue = new ProducerConsumerQueue(2);
        }

        ~TerminalClient()
        {
            Dispose(false);
        }

        public event EventHandler<ClientErrorEventArg> Error;
        public event EventHandler<IPEndPoint> Connected;
        public event EventHandler<IPEndPoint> Disconnected;
        public event EventHandler<TerminalMessage> MessageSent;
        public event EventHandler<TerminalMessage> EventReceived;
        public event EventHandler<TerminalMessage> ResponseReceived;
        public event EventHandler<PulseEventArgs> Pulse;

        public IPEndPoint EndPoint { get; private set; }

        public string HostAddress => EndPoint.Address.ToString();

        public int HostPort => EndPoint.Port;

        public bool IsConnected => _client.Connected;

        public bool Connect(IPEndPoint endPoint)
        {
            return ConnectAsync(endPoint).Result;
        }

        public bool Connect(string address, int port)
        {
            return ConnectAsync(address, port).Result;
        }

        public async Task<bool> ConnectAsync(string address, int port)
        {
            return await ConnectAsync(new IPEndPoint(IPAddress.Parse(address), port));
        }

        public async Task<bool> ConnectAsync(IPEndPoint endPoint)
        {
            bool connected;

            try
            {
                _client = new TcpClient();
                _shutdownToken = new CancellationTokenSource();

                await _client.ConnectAsync(endPoint.Address.ToString(), endPoint.Port);

                EndPoint = endPoint;

#pragma warning disable 4014
                _inboundLoopTask = Task.Factory.StartNew(InboundMessageLoop, _shutdownToken.Token);
#pragma warning restore 4014

                if (!_client.Connected)
                    throw new InvalidOperationException("Connection failed for unknown reason");

                FireConnected();

                _pulseTimer.Change(TimeSpan.FromSeconds(15 * 2), TimeSpan.FromSeconds(15 * 2));

                connected = true;
            }
            catch (Exception ex)
            {
                FireClientError(ClientErrorType.ConnectionError, ex);

                // JIC we were able to connect but encontered another issue
                //Disconnect();

                connected = false;
            }

            return connected;
        }

        public void Disconnect()
        {
            _shutdownToken.Cancel();

            _inboundLoopTask.Wait(TimeSpan.FromSeconds(1));

            _pulseTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

            if (_client.Connected)
            {
                _client.Close();
            }

            EndPoint = null;

            FireDisconnected();
        }

        private void ReConnect()
        {
            IPEndPoint endPoint = EndPoint;

            Disconnect();
            Connect(endPoint);
        }

        public int SendMessage(TerminalMessage message, MessagePriorty priority = MessagePriorty.Normal)
        {
            if (_client == null || !_client.Connected)
                throw new InvalidOperationException("No Connection");

            int sequenceNumber = IncSequenceNumber;

            message.SetCommandSequenceNumber(sequenceNumber);

            _outboundMessageQueue.Enqueue(() => Send(message), _shutdownToken.Token);

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
                _outboundMessageQueue.Dispose();
            }

            _disposed = true;
        }

        private void InboundMessageLoop()
        {
            while (true)
            {
                try
                {
                    if (_shutdownToken.Token.WaitHandle.WaitOne(_inboundLoopSleepTime))
                        break;

                    var reader = _client.GetStream();

                    string xmlMessage;

                    lock (_client)
                    {
                        if (!reader.DataAvailable)
                            continue;

                        byte[] bLength = new byte[4];

                        reader.Read(bLength, 0, bLength.Length);

                        int length = BitConverter.ToInt32(bLength, 0);

                        length = IPAddress.NetworkToHostOrder(length);

                        xmlMessage = ReadBuffer(length);

                        _inboundMessageQueue.Enqueue(() => Receive(xmlMessage), _shutdownToken.Token);
                    }

                }
                catch (Exception ex)
                {
                    FireClientError(ClientErrorType.DataReceiveError, ex);
                    break;
                }
            }
        }

        private void Receive(string xmlMessage)
        {
            if (xmlMessage.Contains("Heartbeat"))
            {
                _pulseTimer.Change(TimeSpan.FromSeconds(15 * 2), TimeSpan.FromSeconds(15 * 2));
                FirePulse(true);
                return;
            }

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

        private void Send(TerminalMessage message)
        {
            try
            {
                byte[] buffer = message.GetBytes();

                var length = IPAddress.HostToNetworkOrder(buffer.Length);

                lock (_client) _client.Client.Send(BitConverter.GetBytes(length).Concat(buffer).ToArray()); 

                FireMessageSent(message);
            }
            catch (Exception ex)
            {
                FireClientError(ClientErrorType.DataSendError, ex);
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
            var reader = _client.GetStream();

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

        private void OnPulseTimerElapsed(object state)
        {
            FirePulse(false);
            ReConnect();
        }

        private void FireMessageSent(TerminalMessage message)
        {
            MessageSent?.Invoke(this, message);
        }

        private void FireConnected()
        {
            Connected?.Invoke(this, EndPoint);
        }

        protected virtual void FireDisconnected()
        {
            Disconnected?.Invoke(this, EndPoint);
        }

        protected virtual void FireEventReceived(TerminalMessage message)
        {
            EventReceived?.Invoke(this, message);
        }

        protected virtual void FireResponseReceived(TerminalMessage message)
        {
            ResponseReceived?.Invoke(this, message);
        }

        protected virtual void FireClientError(ClientErrorType type, Exception ex)
        {
            Error?.Invoke(this, new ClientErrorEventArg {Exception = ex, EndPoint = EndPoint, ErrorType = type, IsError = true});
        }

        protected virtual void FirePulse(bool pulse)
        {
            Pulse?.Invoke(this, new PulseEventArgs {Pulse = pulse, EndPoint = EndPoint});
        }
    }
}
