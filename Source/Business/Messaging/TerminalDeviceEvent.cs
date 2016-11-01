using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.Practices.ServiceLocation;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public sealed class TerminalDeviceEvent : TerminalDeviceMember, ITerminalDeviceEvent
    {
        private List<EventHandler<object>> EventHandlers { get; } = new List<EventHandler<object>>();

        private TerminalDeviceEvent(string name, Type eventType)
            : base(name)
        {
            EventType = eventType;
        }

        public static TerminalDeviceEvent Register<TEvent>(string name, Type ownerType)
            where TEvent : class
        {
            var @event = new TerminalDeviceEvent(name, typeof(TEvent));

            TerminalDevice.AddMember(ownerType, @event);

            return @event;
        }

        public Type EventType { get; }

        public void ClearHandlers()
        {
            EventHandlers.Clear();
        }

        public bool TrySet(object value)
        {
            return this == value;
        }

        public bool TryInvoke(object eventObject)
        {
            // Convert to array because handlers may be unsubscribed while enumerating
            foreach (var handler in EventHandlers.ToArray())
            {
                handler(this, eventObject);
            }

            return true;
        }

        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            result = null;

            if (binder.Operation == ExpressionType.AddAssign)
            {
                if (!AddHandler(arg))
                    return false;
            }
            else if (binder.Operation == ExpressionType.SubtractAssign)
            {
                if (!RemoveHandler(arg))
                    return false;
            }
            else
            {
                return false;
            }

            result = this;

            return true;
        }

        private EventHandler<object> GetEventHandler(object function)
        {
            var resolvers = ServiceLocator.Current.GetAllInstances<ITerminalDeviceEventHandlerResolver>();

            if (resolvers == null)
                return null;

            EventHandler<object> handler = null;

            foreach (var resolver in resolvers)
            {
                handler = resolver.GetEventHandler(function);

                if (handler != null)
                    break;
            }

            return handler;
        }

        private bool AddHandler(object function)
        {
            EventHandler<object> handler = GetEventHandler(function);

            if (handler == null)
                return false;

            if (!EventHandlers.Contains(handler))
                EventHandlers.Add(handler);

            return true;
        }

        private bool RemoveHandler(object function)
        {
            EventHandler<object> handler = GetEventHandler(function);

            if (function == null)
                return false;

            EventHandlers.Remove(handler);

            return true;
        }
    }
}
