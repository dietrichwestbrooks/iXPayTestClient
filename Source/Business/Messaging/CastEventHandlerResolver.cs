using System;
using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    [Export(typeof(ITerminalDeviceEventHandlerResolver))]
    internal class CastEventHandlerResolver : ITerminalDeviceEventHandlerResolver
    {
        public EventHandler<object> GetEventHandler(object function)
        {
            return function as EventHandler<object>;
        }
    }
}
