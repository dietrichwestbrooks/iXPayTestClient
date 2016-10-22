using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Services
{
    public class ScriptHost
    {
        public ScriptHost()
        {
            EventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        }

        public void Write(string text)
        {
            EventAggregator.GetEvent<OutputTextEvent>().Publish(new OutputTextEventArgs
                {
                    Category = OutputTextCategory.Script,
                    Text = text
                });
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

        private IEventAggregator EventAggregator { get; }
    }
}
