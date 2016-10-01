using System;
using System.Net;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public class PulseEventArgs : EventArgs
    {
        public bool Pulse { get; set; }

        public IPEndPoint EndPoint { get; set; }
    }
}
