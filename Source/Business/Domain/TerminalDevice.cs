using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public abstract class TerminalDevice : DynamicObject, ITerminalDevice
    {
        private Stack<string> _callStack = new Stack<string>();
         
        protected Type CommandType { get; set; }
        protected Type ResponseType { get; set; }

        protected TerminalDevice()
        {
            Name = GetType().Name;
        }

        public string Name { get; protected set; }

        public ITerminalDeviceMethodCollection Methods { get; } = new TerminalDeviceMethodCollection();

        public ITerminalDevicePropertyCollection Properties { get; } = new TerminalDevicePropertyCollection();

        public virtual ITerminalRequestHandler Successor { get; protected set; }

        public virtual object HandleRequest(object command)
        {
            var outerCommand = Activator.CreateInstance(CommandType);

            outerCommand.GetType().InvokeMember("Item",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, outerCommand, new[] { command });

            return outerCommand;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            var property = Properties.FirstOrDefault(p => p.Name == binder.Name);

            if (property != null)
                return TryInvoke(property, $"get_{property.Name}", null, null, out result);

            property = Properties.FirstOrDefault(p => $"get_{p.Name}" == binder.Name || $"set_{p.Name}" == binder.Name);

            if (property != null)
            {
                _callStack.Push(binder.Name);
                result = this;
                return true;
            }

            var method = Methods.FirstOrDefault(m => m.Name == binder.Name);

            if (method != null)
            {
                _callStack.Push(method.Name);
                result = this;
                return true;
            }

            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var property = Properties.FirstOrDefault(p => p.Name == binder.Name);

            if (property == null)
                return false;

            object result;

            return TryInvoke(property, $"set_{property.Name}", new[] {"value"}, new[] {value}, out result);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;

            string methodName = binder.Name;

            ITerminalDeviceProperty property = Properties.FirstOrDefault(p => $"get_{p.Name}" == methodName || $"set_{p.Name}" == methodName);

            if (property != null)
                return TryInvoke(property, methodName, binder.CallInfo.ArgumentNames, args, out result);

            ITerminalDeviceMethod method = Methods.FirstOrDefault(m => m.Name == methodName);

            if (method != null)
                return TryInvoke(method, methodName, binder.CallInfo.ArgumentNames, args, out result);

            return false;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            result = null;

            string methodName = _callStack.Peek();

            ITerminalDeviceProperty property = Properties.FirstOrDefault(p => $"get_{p.Name}" == methodName || $"set_{p.Name}" == methodName);

            if (property != null)
            {
                _callStack.Pop();
                return TryInvoke(property, methodName, binder.CallInfo.ArgumentNames, args, out result);
            }

            ITerminalDeviceMethod method = Methods.FirstOrDefault(m => m.Name == methodName);

            if (method != null)
            {
                _callStack.Pop();
                return TryInvoke(method, methodName, binder.CallInfo.ArgumentNames, args, out result);
            }

            return false;
        }

        protected bool TryInvoke(ITerminalDeviceCommand command, string methodName, IReadOnlyCollection<string> argNames, object[] args, out object result)
        {
            result = null;

            try
            {
                var parameters = new CommandParameters();

                // todo change to work with unnamed parameters as well
                if (argNames != null && args != null)
                {
                    foreach (var parameter in argNames.Zip(args, (name, arg) => new { Name = name, Value = arg }))
                    {
                        parameters.Add(parameter.Name, parameter.Value);
                    } 
                }

                if (methodName.StartsWith("set_"))
                {
                    var property = (ITerminalDeviceProperty)command;
                    TerminalMessage message = property.GetSetMessage(parameters);
                    object response = SendMessageAsync(message).Result;
                    property.ProcessSetResponse(response);
                }
                else if (methodName.StartsWith("get_"))
                {
                    var property = (ITerminalDeviceProperty)command;
                    TerminalMessage message = property.GetGetMessage(parameters);
                    object response = SendMessageAsync(message).Result;
                    property.ProcessGetResponse(response);
                }
                else
                {
                    var method = (ITerminalDeviceMethod)command;
                    TerminalMessage message = method.GetInvokeMessage(parameters);
                    object response = SendMessageAsync(message).Result;
                    method.ProcessInvokeResponse(response);
                }

                if (!command.Result)
                    throw new InvalidOperationException(command.ResultMessage);

                result = command.Result;

                return true;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        protected void Set(string propName, IDictionary<string, object> parameters)
        {
            ITerminalDeviceProperty property = Properties.First(p => p.Name == propName);

            if (property == null)
                throw new InvalidOperationException($"'{Name}' object has no property '{propName}'");

            object result;

            TryInvoke(property, $"set_{propName}", parameters.Keys.ToArray(), parameters.Values.ToArray(), out result);

            if (!property.Result)
                throw new InvalidOperationException(property.ResultMessage);
        }

        protected void Get(string propName, IDictionary<string, object> parameters)
        {
            ITerminalDeviceProperty property = Properties.First(p => p.Name == propName);

            if (property == null)
                throw new InvalidOperationException($"'{Name}' object has no property '{propName}'");

            object result;

            TryInvoke(property, $"get_{propName}", parameters.Keys.ToArray(), parameters.Values.ToArray(), out result);

            if (!property.Result)
                throw new InvalidOperationException(property.ResultMessage);
        }

        protected void Invoke(string methodName, IDictionary<string, object> parameters)
        {
            ITerminalDeviceMethod method = Methods.First(p => p.Name == methodName);

            if (method == null)
                throw new InvalidOperationException($"'{Name}' object has no method '{methodName}'");

            object result;

            TryInvoke(method, methodName, parameters.Keys.ToArray(), parameters.Values.ToArray(), out result);

            if (!method.Result)
                throw new InvalidOperationException(method.ResultMessage);
        }

        protected Task<object> SendMessageAsync(TerminalMessage message)
        {
            return new MessageBrokerTask(message).Task;
        }
    }

    public abstract class TerminalDeviceT<TCommand, TResponse> : TerminalDevice
    {
        protected TerminalDeviceT()
        {
            CommandType = typeof (TCommand);
            ResponseType = typeof (TResponse);
        }
    }
}
