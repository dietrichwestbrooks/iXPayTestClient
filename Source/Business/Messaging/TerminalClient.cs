using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    [Export(typeof(ITerminalClient))]
    internal sealed class TerminalClient : TerminalConnection, ITerminalClient
    {
        private Timer _pulseTimer;
        private bool _keepAlive;
        private bool _isSecure;

        public TerminalClient()
        {
            _pulseTimer = new Timer(OnPulseTimerElapsed, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        public async Task ConnectAsync(IPEndPoint endPoint, bool isSecure, bool keepAlive)
        {
            try
            {
                var client = new TcpClient();

                await client.ConnectAsync(endPoint.Address.ToString(), endPoint.Port);

                if (!client.Connected)
                    throw new InvalidOperationException("Connection failed");

                EndPoint = endPoint;

                _isSecure = isSecure;
                _keepAlive = keepAlive;

                Connect(client);
            }
            catch (Exception ex)
            {
                FireError(ClientErrorType.ConnectionError, ex);

                // JIC we were able to connect but encontered another issue
                //Disconnect();
            }
        }

        protected override void OnConnected()
        {
            _pulseTimer.Change(TimeSpan.FromSeconds(15 * 2), TimeSpan.FromSeconds(15 * 2));
        }

        protected override void OnDisconnected()
        {
            _pulseTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        protected override void Receive(string xmlMessage)
        {
            if (xmlMessage.Contains("Heartbeat"))
            {
                _pulseTimer.Change(TimeSpan.FromSeconds(15 * 2), TimeSpan.FromSeconds(15 * 2));
                FirePulse();
                return;
            }

            base.Receive(xmlMessage);
        }

        private void Reconnect()
        {
            try
            {
                while (!ShutdownToken.Token.WaitHandle.WaitOne(100))
                {
                    if (Client.Connected)
                        Client.Client.Disconnect(true);

                    Client.Connect(EndPoint.Address.ToString(), EndPoint.Port);

                    if (Client.Connected)
                    {
                        _pulseTimer.Change(TimeSpan.FromSeconds(15 * 2), TimeSpan.FromSeconds(15 * 2));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                FireError(ClientErrorType.ConnectionError, ex);
                Disconnect();
            }
        }

        private void OnPulseTimerElapsed(object state)
        {
            _pulseTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            if (_keepAlive)
                using (new TerminalConnectionLocker()) Reconnect();
            else
                Disconnect();
        }
    }
}
