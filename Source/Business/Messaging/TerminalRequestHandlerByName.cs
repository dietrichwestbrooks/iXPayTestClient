using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public class TerminalRequestHandlerByName : ITerminalRequestHandler
    {
        public TerminalRequestHandlerByName(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public ITerminalRequestHandler Successor
        {
            get
            {
                var handler = ServiceLocator.Current.GetAllInstances<ITerminalRequestHandler>()
                        .OfType<INamedObject>()
                        .FirstOrDefault(h => h.Name == Name) as ITerminalRequestHandler;

                return handler?.Successor;
            }
            set
            {
                throw new InvalidOperationException("Setting successor is not allowed on this object");
            }
        }

        public object HandleRequest(object command)
        {
            var handler = ServiceLocator.Current.GetAllInstances<ITerminalRequestHandler>()
                    .OfType<INamedObject>()
                    .FirstOrDefault(h => h.Name == Name) as ITerminalRequestHandler;

            if (handler == null)
                return command;

            return handler.HandleRequest(command);
        }
    }
}
