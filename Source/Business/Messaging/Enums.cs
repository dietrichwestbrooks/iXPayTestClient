﻿namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public enum MessagePriorty
    {
        Low,
        Normal,
        High,
    }

    public enum ClientErrorType
    {
        ConnectionError,
        DataSendError,
        DataReceiveError,
    }
}
