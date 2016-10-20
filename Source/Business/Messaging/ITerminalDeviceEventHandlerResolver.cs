using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDeviceEventHandlerResolver
    {
        EventHandler<object> GetEventHandler(object function);
    }
}
