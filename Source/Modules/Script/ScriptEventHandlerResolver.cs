using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script
{
    [Export(typeof(ITerminalDeviceEventHandlerResolver))]
    public class ScriptEventHandlerResolver : ITerminalDeviceEventHandlerResolver
    {
        public EventHandler<object> GetEventHandler(object function)
        {
            var scriptService = ServiceLocator.Current.GetInstance<IScriptService>();
            return scriptService.GetFunction<EventHandler<object>>(function);
        }
    }
}
