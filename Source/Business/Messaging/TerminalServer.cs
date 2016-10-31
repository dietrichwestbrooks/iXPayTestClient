using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    [Export(typeof(ITerminalServer))]
    internal sealed class TerminalServer : TerminalConnection, ITerminalServer
    {
        private Timer _pulseTimer;
        private TcpListener _listener;
        private bool _isSecure;

        public TerminalServer()
        {
            _pulseTimer = new Timer(OnPulseTimerElapsed, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        public async Task StartAsync(IPEndPoint endPoint, bool isSecure)
        {
            try
            {
                EndPoint = endPoint;

                _isSecure = isSecure;

                _listener = new TcpListener(endPoint);

                await Listen();
            }
            catch (Exception ex)
            {
                FireError(ClientErrorType.ConnectionError, ex);

                // JIC we were able to connect but encontered another issue
                //Disconnect();
            }
        }

        private async Task Listen()
        {
            _listener.Start();

            TcpClient client = await _listener.AcceptTcpClientAsync();

            Connect(client);
        }

        protected override void OnConnected()
        {
            _listener.Stop();

            //_pulseTimer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(15));
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

        private void OnPulseTimerElapsed(object state)
        {
            _pulseTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

            Listen().Wait(ShutdownToken.Token);

            //SendMessage(new TerminalMessage
            //    {
            //        Item = new TerminalEvent
            //            {
            //                Item = new Heartbeat()
            //            }
            //    });
        }
    }
}
