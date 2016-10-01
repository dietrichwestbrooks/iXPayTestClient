using System;
using System.Net;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events
{
    public class ConnectionEventArgs : EventArgs
    {
        public ConnectionEventType Type { get; set; }

        public IPEndPoint EndPoint { get; set; }

        public bool Pulse { get; set; }

        public Exception Exception { get; set; }
    }
}
