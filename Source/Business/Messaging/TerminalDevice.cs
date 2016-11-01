using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public sealed class TerminalDevice : DynamicObject, ITerminalDevice
    {
        private static Dictionary<Type, List<ITerminalDeviceMember>> RegisteredMembers { get; } =
          new Dictionary<Type, List<ITerminalDeviceMember>>();

        private Type _ownerType;

        private TerminalDevice(string name, ITerminalRequestHandler successor, Type commandType, Type responseType, Type eventType = null)
        {
            Name = name;
            Successor = successor;
            CommandType = commandType;
            ResponseType = responseType;
            EventType = eventType;
        }

        public static TerminalDevice Register<TCommand, TResponse, TEvent>(string name, ITerminalRequestHandler successor, Type ownerType)
        {
            var device = new TerminalDevice(name, successor, typeof(TCommand), typeof(TResponse), typeof(TEvent));

            RegisterCommon(ownerType, device);

            return device;
        }

        public static TerminalDevice Register<TCommand, TResponse>(string name, ITerminalRequestHandler successor, Type ownerType)
        {
            var device = new TerminalDevice(name, successor, typeof(TCommand), typeof(TResponse));

            RegisterCommon(ownerType, device);

            return device;
        }

        public static TerminalDevice Register<TCommand, TResponse, TEvent>(string name, Type ownerType)
        {
            return Register<TCommand, TResponse, TEvent>(name, null, ownerType);
        }

        public static TerminalDevice Register<TCommand, TResponse>(string name, Type ownerType)
        {
            return Register<TCommand, TResponse>(name, null, ownerType);
        }

        private static void RegisterCommon(Type ownerType, TerminalDevice device)
        {
            AddOwnerType(ownerType, device);

            var container = ServiceLocator.Current.GetInstance<CompositionContainer>();
            container.ComposeExportedValue<ITerminalDevice>(device);
            container.ComposeExportedValue<ITerminalRequestHandler>(device);
        }

        public string Name { get; }

        public Type CommandType { get; }

        public Type ResponseType { get; }

        public Type EventType { get; }

        public IEnumerable<ITerminalDeviceMethod> Methods =>
            RegisteredMembers[_ownerType].OfType<ITerminalDeviceMethod>();

        public IEnumerable<ITerminalDeviceProperty> Properties =>
            RegisteredMembers[_ownerType].OfType<ITerminalDeviceProperty>();

        public IEnumerable<ITerminalDeviceEvent> Events =>
            RegisteredMembers[_ownerType].OfType<ITerminalDeviceEvent>();

        public void ClearEventHandlers()
        {
            foreach (var e in Events)
            {
                e.ClearHandlers();
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

            TerminalDeviceEvent @event = Events.FirstOrDefault(e => e.Name == binder.Name) as TerminalDeviceEvent;

            if (@event != null)
                return @event.TrySet(value);

            return false;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;

            string methodName = binder.Name;

            TerminalDeviceProperty property = Properties.FirstOrDefault(p => p.GetCommand?.Name == methodName) as TerminalDeviceProperty;

            if (property != null)
                return property.TryGet(new CommandParameters(binder.CallInfo.ArgumentNames, args), out result);

            property = Properties.FirstOrDefault(p => p.SetCommand?.Name == methodName) as TerminalDeviceProperty;

            if (property != null)
                return property.TrySet(new CommandParameters(binder.CallInfo.ArgumentNames, args));

            TerminalDeviceMethod method = Methods.FirstOrDefault(m => m.Name == methodName) as TerminalDeviceMethod;

            if (method != null)
                return method.TryInvoke(new CommandParameters(binder.CallInfo.ArgumentNames, args), out result);

            return false;
        }

        public ITerminalRequestHandler Successor { get; set; }

        public object HandleRequest(object command)
        {
            var outerCommand = Activator.CreateInstance(CommandType);

            outerCommand.GetType().InvokeMember("Item",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, outerCommand, new[] { command });

            return outerCommand;
        }

        private static void AddOwnerType(Type ownerType, TerminalDevice device)
        {
            AddOwnerType(ownerType);
            device._ownerType = ownerType;

            foreach (var member in RegisteredMembers[ownerType].Cast<TerminalDeviceMember>())
            {
                member.Attach(device);
            }
        }

        private static void AddOwnerType(Type ownerType)
        {
            if (!RegisteredMembers.ContainsKey(ownerType))
                RegisteredMembers.Add(ownerType, new List<ITerminalDeviceMember>());
        }

        public static void AddMember(Type ownerType, ITerminalDeviceMember member)
        {
            AddOwnerType(ownerType);
            RegisteredMembers[ownerType].Add(member);
        }
    }
}
