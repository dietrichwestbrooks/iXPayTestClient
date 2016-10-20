using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDeviceEvent : ITerminalDeviceMember
    {
        Type EventType { get; }
        bool TryInvoke(object eventObject);
        bool TrySet(object value);
        void ClearHandlers();
    }
}
