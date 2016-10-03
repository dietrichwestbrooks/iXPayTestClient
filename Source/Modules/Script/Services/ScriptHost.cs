using System;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Services
{
    public class ScriptHost
    {
        public ScriptHost()
        {
            EventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        }

        public void Write(string message)
        {
            EventAggregator.GetEvent<ScriptOutputEvent>().Publish(message);
        }

        public void WriteIf(bool condition, string message)
        {
            if (condition)
                Write(message);
        }

        public void WriteLine(string message)
        {
            Write($"{message}\n");
        }

        public void WriteLineIf(bool condition, string message)
        {
            if (condition)
                WriteLine(message);
        }

        public void RegisterDevice(ITerminalDevice device)
        {
            var terminalService = ServiceLocator.Current.GetInstance<ITerminalClientService>();

            if (terminalService == null)
                throw new InvalidOperationException("Unable to locate terminal service");

            terminalService.RegisterDevice(device);
        }

        private IEventAggregator EventAggregator { get; }
    }
}
