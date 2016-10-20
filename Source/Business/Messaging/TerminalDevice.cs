using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public abstract class TerminalDevice<TCommand, TResponse> : DynamicObject, ITerminalDevice
    {
        protected TerminalDevice(string name)
        {
            Name = name;

            CommandType = typeof(TCommand);
            ResponseType = typeof(TResponse);

            var client = ServiceLocator.Current.GetInstance<ITerminalClient>();
            client.EventReceived += OnEventReceived;
        }

        public string Name { get; protected set; }

        public Type CommandType { get; set; }
        public Type ResponseType { get; set; }
        public Type EventType { get; set; }

        public TerminalDeviceMethodCollection Methods { get; } = new TerminalDeviceMethodCollection();

        public TerminalDevicePropertyCollection Properties { get; } = new TerminalDevicePropertyCollection();

        public TerminalDeviceEventCollection Events { get; } = new TerminalDeviceEventCollection();

        public virtual ITerminalRequestHandler Successor { get; protected set; }

        public virtual object HandleRequest(object command)
        {
            var outerCommand = Activator.CreateInstance(CommandType);

            outerCommand.GetType().InvokeMember("Item",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, outerCommand, new[] { command });

            return outerCommand;
        }

        public void ClearEventHandlers()
        {
            foreach (var @event in Events)
            {
                @event.ClearHandlers();
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            var property = Properties.FirstOrDefault(p => p.Name == binder.Name);

            if (property != null)
                return property.TryGet(new CommandParameters(), out result);

            property = Properties.FirstOrDefault(p => p.GetCommand?.Name == binder.Name);

            if (property != null)
            {
                property.InvokeFlag = PropertyInvoke.Get;
                result = property;
                return true;
            }

            property = Properties.FirstOrDefault(p => p.SetCommand?.Name == binder.Name);

            if (property != null)
            {
                property.InvokeFlag = PropertyInvoke.Set;
                result = property;
                return true;
            }

            var method = Methods.FirstOrDefault(m => m.Name == binder.Name);

            if (method != null)
            {
                result = method;
                return true;
            }

            var @event = Events.FirstOrDefault(e => e.Name == binder.Name);

            if (@event != null)
            {
                result = @event;
                return true;
            }

            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var property = Properties.FirstOrDefault(p => p.Name == binder.Name);

            if (property != null)
                return property.TrySet(new CommandParameters(new[] { "value" }, new[] { value }));

            ITerminalDeviceEvent @event = Events.FirstOrDefault(e => e.Name == binder.Name);

            if (@event != null)
                return @event.TrySet(value);

            return false;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;

            string methodName = binder.Name;

            ITerminalDeviceProperty property = Properties.FirstOrDefault(p => p.GetCommand?.Name == methodName);

            if (property != null)
                return property.TryGet(new CommandParameters(binder.CallInfo.ArgumentNames, args), out result);
            
            property = Properties.FirstOrDefault(p => p.SetCommand?.Name == methodName);

            if (property != null)
                return property.TrySet(new CommandParameters(binder.CallInfo.ArgumentNames, args));

            ITerminalDeviceMethod method = Methods.FirstOrDefault(m => m.Name == methodName);

            if (method != null)
                return method.TryInvoke(new CommandParameters(binder.CallInfo.ArgumentNames, args), out result);

            return false;
        }

        private void OnEventReceived(object sender, TerminalMessage message)
        {
            if (EventType == null)
                return;

            var eventContainerObject = message.GetSecondToLastItem();

            if (eventContainerObject == null || eventContainerObject.GetType() != EventType)
                return;

            var eventObject = message.GetLastItem();

            var @event = Events.FirstOrDefault(e => eventObject.GetType() == e.EventType);

            @event?.TryInvoke(eventObject);
        }
    }

    public abstract class TerminalDevice<TCommand, TResponse, TEvent> : TerminalDevice<TCommand, TResponse>
    {
        protected TerminalDevice(string name)
            : base(name)
        {
            EventType = typeof(TEvent);
        }
    }
}
