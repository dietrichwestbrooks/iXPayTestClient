using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.New
{
    public class TerminalDevice : DynamicObject, ITerminalDevice
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

        protected static void AddOwnerType(Type ownerType, TerminalDevice device)
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

    public class TerminalDeviceMember : DynamicObject
    {
        public ITerminalDevice Device { get; private set; }

        public void Attach(TerminalDevice device)
        {
            if (Device == device)
                return;

            Device = device;
        }
    }

    public sealed class TerminalDeviceEvent : TerminalDeviceMember, ITerminalDeviceEvent
    {
        private List<EventHandler<object>> EventHandlers { get; } = new List<EventHandler<object>>();

        private TerminalDeviceEvent(string name, Type eventType)
        {
            Name = name;
            EventType = eventType;
        }

        public static TerminalDeviceEvent Register<TEvent>(string name, Type ownerType)
            where TEvent : class
        {
            var @event = new TerminalDeviceEvent(name, typeof(TEvent));

            TerminalDevice.AddMember(ownerType, @event);

            return @event;
        }

        public string Name { get; }

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

    public class TerminalDeviceMethod : TerminalDeviceMember, ITerminalDeviceMethod
    {
        public TerminalDeviceMethod(string name, Type commandType, Type responseType, Func<object> prepareCommandFunc)
        {
            Name = name;
            CommandType = commandType;
            ResponseType = responseType;
            InvokeCommand = new TerminalDeviceCommand(this, name, CommandType, ResponseType, prepareCommandFunc);
        }

        public static TerminalDeviceMethod Register<TCommand, TResponse>(string name, Type ownerType, Func<TCommand> prepareCommandFunc = null)
            where TCommand : class
            where TResponse : class
        {
            var method = new TerminalDeviceMethod(name, typeof(TCommand), typeof(TResponse), prepareCommandFunc);

            TerminalDevice.AddMember(ownerType, method);

            return method;
        }

        public string Name { get; }

        public Type CommandType { get; }

        public Type ResponseType { get; }

        public ITerminalDeviceCommand InvokeCommand { get; }

        public virtual bool TryInvoke(CommandParameters parameters, out object result)
        {
            result = InvokeCommand.Execute(parameters);
            return true;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return TryInvoke(new CommandParameters(binder.CallInfo.ArgumentNames, args), out result);
        }
    }

    public sealed class TerminalDeviceProperty : TerminalDeviceMember, ITerminalDeviceProperty
    {
        private TerminalDeviceProperty(string name, Type valueType, Type getCommandType, Type getResponseType, Func<object> prepareGetCommandFunc, Type setCommandType, Type setResponseType, Func<object> prepareSetCommandFunc)
        {
            Name = name;
            ValueType = valueType;
            GetCommandType = getCommandType;
            GetResponseType = getResponseType;
            GetCommand = new TerminalDeviceCommand(this, $"get_{Name}", GetCommandType, GetResponseType, prepareGetCommandFunc);
            SetCommandType = setCommandType;
            SetResponseType = setResponseType;
            SetCommand = new TerminalDeviceCommand(this, $"set_{Name}", SetCommandType, SetResponseType, prepareSetCommandFunc);
        }

        private TerminalDeviceProperty(string name, Type valueType, Type getCommandType, Type getResponseType, Func<object> prepareGetCommandFunc)
        {
            Name = name;
            ValueType = valueType;
            GetCommandType = getCommandType;
            GetResponseType = getResponseType;
            GetCommand = new TerminalDeviceCommand(this, $"get_{Name}", GetCommandType, GetResponseType, prepareGetCommandFunc);
        }

        public static TerminalDeviceProperty Register<TValue, TGetCommand, TGetResponse, TSetCommand, TSetResponse>(
            string name, Type ownerType, Func<TGetCommand> prepareGetCommandFunc,
            Func<TSetCommand> prepareSetCommandFunc)
            where TGetCommand : class
            where TGetResponse : class
            where TSetCommand : class
            where TSetResponse : class
        {
            var property = new TerminalDeviceProperty(name, typeof (TValue), typeof (TGetCommand), typeof (TGetResponse),
                prepareGetCommandFunc, typeof (TSetCommand), typeof (TSetResponse), prepareSetCommandFunc);

            RegisterCommon(ownerType, property);

            return property;
        }

        public static TerminalDeviceProperty Register<TValue, TGetCommand, TGetResponse>(string name, Type ownerType,
            Func<TGetCommand> prepareGetCommandFunc = null)
            where TGetCommand : class
            where TGetResponse : class
        {
            var property = new TerminalDeviceProperty(name, typeof (TValue), typeof (TGetCommand), typeof (TGetResponse),
                prepareGetCommandFunc);

            RegisterCommon(ownerType, property);

            return property;
        }

        private static void RegisterCommon(Type ownerType, TerminalDeviceProperty property)
        {
            TerminalDevice.AddMember(ownerType, property);
        }

        public string Name { get; }

        public Type ValueType { get; }

        public Type GetCommandType { get; }

        public Type GetResponseType { get; }

        public Type SetCommandType { get; }

        public Type SetResponseType { get; }

        public ITerminalDeviceCommand GetCommand { get; }

        public ITerminalDeviceCommand SetCommand { get; }

        public PropertyInvoke InvokeFlag { get; set; }

        public bool TryGet(CommandParameters parameters, out object result)
        {
            var response = GetCommand.Execute(parameters);

            object value = GetValue(response);

            result = InvokeFlag == PropertyInvoke.Get ? response : value;

            return true;
        }

        private object GetValue(object response)
        {
            PropertyInfo valueProperty = null;

            var attribute = GetType().GetCustomAttributes(typeof(ValuePropertyAttribute), false).FirstOrDefault() as ValuePropertyAttribute;

            if (attribute != null)
            {
                valueProperty = response.GetType()
                        .GetProperties()
                        .FirstOrDefault(p =>
                                string.Equals(p.Name, attribute.PropertyName, StringComparison.CurrentCultureIgnoreCase) &&
                                p.PropertyType == ValueType);
            }

            if (valueProperty == null)
            {
                valueProperty = response.GetType()
                        .GetProperties()
                        .FirstOrDefault(p => p.Name.ToLower() == "value" && p.PropertyType == ValueType);
            }

            if (valueProperty == null)
            {
                valueProperty = response.GetType()
                        .GetProperties()
                        .FirstOrDefault(p => p.PropertyType == ValueType);
            }

            return valueProperty?.GetValue(response);
        }

        public bool TrySet(CommandParameters parameters)
        {
            object response;
            TrySet(parameters, out response);
            return true;
        }

        private bool TrySet(CommandParameters parameters, out object result)
        {
            SetValue(parameters);
            result = SetCommand.Execute(parameters);
            return true;
        }

        private void SetValue(CommandParameters parameters)
        {
            PropertyInfo valueProperty = null;

            var value = parameters.Where(p => p.Key.ToLower() == "value").Select(p => p.Value).FirstOrDefault();

            parameters.Remove("value");

            if (value == null)
                return;

            var attribute = GetType().GetCustomAttributes(typeof(ValuePropertyAttribute), false).FirstOrDefault() as ValuePropertyAttribute;

            if (attribute != null)
            {
                valueProperty = SetCommand.CommandType
                        .GetProperties()
                        .FirstOrDefault(p =>
                                string.Equals(p.Name, attribute.PropertyName, StringComparison.CurrentCultureIgnoreCase) &&
                                p.PropertyType == ValueType);
            }

            if (valueProperty == null)
            {
                valueProperty = SetCommand.CommandType
                        .GetProperties()
                        .FirstOrDefault(p => p.Name.ToLower() == "value" && p.PropertyType == ValueType);
            }

            if (valueProperty == null)
            {
                valueProperty = SetCommand.CommandType
                        .GetProperties()
                        .FirstOrDefault(p => p.PropertyType == ValueType);
            }

            if (valueProperty == null)
                return;

            parameters.Add(valueProperty.Name, value);
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            result = null;

            bool success = false;

            try
            {
                switch (InvokeFlag)
                {
                    case PropertyInvoke.Get:
                        success = TryGet(new CommandParameters(binder.CallInfo.ArgumentNames, args), out result);
                        break;

                    case PropertyInvoke.Set:
                        success = TrySet(new CommandParameters(binder.CallInfo.ArgumentNames, args), out result);
                        break;
                }
            }
            finally
            {
                InvokeFlag = PropertyInvoke.None;
            }

            return success;
        }
    }

    public class TerminalDeviceCommand : ITerminalDeviceCommand
    {
        private readonly Func<object> _prepareCommandFunc;

        public TerminalDeviceCommand(ITerminalDeviceMember member, string name, Type commandType, Type responseType, Func<object> prepareCommandFunc)
        {
            Member = member;
            //Successor = Successor;
            Name = name;
            CommandType = commandType;
            ResponseType = responseType;
            _prepareCommandFunc = prepareCommandFunc ?? (() => Activator.CreateInstance(CommandType));
        }

        public string Name { get; set; }

        public Type CommandType { get; }

        public Type ResponseType { get; }

        public bool Result { get; private set; }

        public string ResultMessage { get; private set; }

        public ITerminalDeviceMember Member { get; }

        public ITerminalRequestHandler Successor { get; set; }

        public object HandleRequest(object command)
        {
            return command;
        }

        public TerminalMessage GetMessage(CommandParameters parameters = null)
        {
            object command = (parameters == null ? _prepareCommandFunc() : CreateCommand(parameters));

            return new TerminalMessage { Item = BuildCommand(command, Member.Device) };
            //return new TerminalMessage { Item = BuildCommand(command, Successor) };
        }

        public object Execute(CommandParameters parameters)
        {
            object response;

            try
            {
                TerminalMessage message = GetMessage(parameters);

                response = SendMessageAsync(message).Result;

                if (!ProcessResponse(response))
                    throw new InvalidOperationException(ResultMessage);
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException ?? ex;
            }

            return response;
        }

        protected virtual Task<object> SendMessageAsync(TerminalMessage message)
        {
            return new MessageBrokerTask(message).Task;
        }

        private object CreateCommand(CommandParameters parameters)
        {
            var command = Activator.CreateInstance(CommandType);

            PropertyInfo[] properties = CommandType.GetProperties();

            var namedParams = parameters.Where(p => !string.IsNullOrWhiteSpace(p.Key));

            // Match unnamed parameters with command properties by position and assign property name
            var unamedParams = parameters
                .Where(p => string.IsNullOrWhiteSpace(p.Key))
                .Zip(properties, (param, prop) => new KeyValuePair<string, object>(prop.Name, param.Value));

            var allParams = unamedParams.Concat(namedParams);

            foreach (var parameter in allParams)
            {
                var property =
                    properties.FirstOrDefault(
                        p => string.Equals(p.Name, parameter.Key, StringComparison.InvariantCultureIgnoreCase));

                property?.SetValue(command, parameter.Value);
            }

            return command;
        }

        private object BuildCommand(object commandItem, ITerminalRequestHandler successor)
        {
            while (true)
            {
                if (successor == null)
                    return commandItem;

                var command = successor.HandleRequest(commandItem);

                commandItem = command;

                successor = successor.Successor;
            }
        }

        private bool ProcessResponse(object response)
        {
            BaseResponse baseResponse = response as BaseResponse;

            if (baseResponse != null)
                return ProcessResponse(baseResponse);

            BaseSimpleHexResponse hexResponse = response as BaseSimpleHexResponse;

            if (hexResponse != null)
                return ProcessResponse(hexResponse);

            return false;
        }

        private bool ProcessResponse(BaseResponse response)
        {
            ResultMessage = response.Message;
            return (Result = response.Success);
        }

        private bool ProcessResponse(BaseSimpleHexResponse response)
        {
            ResultMessage = response.Message;
            return (Result = response.Success);
        }
    }

    public class SamReader
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<SAMReaderCommand, SAMReaderResponse, SAMReaderEvent>(
                    "SAMReader", new TerminalRequestHandlerByName("Terminal"), typeof(SamReader));
        }

        public static readonly TerminalDeviceProperty StatusProperty =
            TerminalDeviceProperty.Register<Status, GetStatusCommand, GetStatusResponse>("Status",
                typeof(SamReader));

        public static readonly TerminalDeviceProperty AvailableSlotsProperty =
            TerminalDeviceProperty.Register<SAMSlot, GetAvailableSlotsCommand, GetAvailableSlotsResponse>("AvailableSlots",
                typeof(SamReader));

        public static readonly TerminalDeviceMethod RefreshAvailableSlotsMethod =
            TerminalDeviceMethod.Register<RefreshAvailableSlotsCommand, RefreshAvailableSlotsResponse>("RefreshAvailableSlots",
                typeof(SamReader));

        public static readonly TerminalDeviceMethod SelectSamSlotMethod =
            TerminalDeviceMethod.Register<SelectSAMSlotCommand, SelectSAMSlotResponse>("SelectSAMSlot",
                typeof(SamReader), () => new SelectSAMSlotCommand {SlotID = 4});

        public static readonly TerminalDeviceMethod ActivateSamMethod =
            TerminalDeviceMethod.Register<ActivateSAMCommand, ActivateSAMResponse>("ActivateSAM",
                typeof(SamReader));

        public static readonly TerminalDeviceMethod DeactivateSamMethod =
            TerminalDeviceMethod.Register<DeactivateSAMCommand, DeactivateSAMResponse>("DeactivateSAM",
                typeof(SamReader));

        public static readonly TerminalDeviceMethod SoftResetSamMethod =
            TerminalDeviceMethod.Register<SoftResetSAMCommand, SoftResetSAMResponse>("SoftResetSAM",
                typeof(SamReader));

        public static readonly TerminalDeviceMethod SamProcessApduMethod =
            TerminalDeviceMethod.Register<SAMProcessAPDUCommand, SAMProcessAPDUResponse>("SAMProcessAPDU",
                typeof(SamReader));

        public static readonly TerminalDeviceEvent StatusChangedEvent =
            TerminalDeviceEvent.Register<StatusChanged>("StatusChanged", typeof(SamReader));
    }
}
