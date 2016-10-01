namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public static class Enums
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
}
