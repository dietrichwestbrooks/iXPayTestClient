using System;
using System.Net;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public class ClientErrorEventArg : EventArgs
    {
        public bool IsError { get; set; }

        public IPEndPoint EndPoint { get; set; }

        public Enums.ClientErrorType ErrorType { get; set; }

        public Exception Exception { get; set; }
    }
}
