using System;
using System.Net;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public class ConnectionErrorEventArgs : EventArgs
    {
        public bool IsError { get; set; }

        public IPEndPoint EndPoint { get; set; }

        public ClientErrorType ErrorType { get; set; }

        public Exception Exception { get; set; }
    }
}
