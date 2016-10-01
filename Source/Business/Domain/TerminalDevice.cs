using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
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

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            try
            {
                var property = Properties.FirstOrDefault(p => p.Name == binder.Name);

                if (property == null)
                {
                    // Check to see if it is a method call
                    var method = Methods.FirstOrDefault(m => m.Name == binder.Name);

                    if (method == null)
                        return false;

                    _callStack.Push(binder.Name);

                    result = this;

                    return true;
                }

                TerminalMessage message = property.GetGetMessage(new CommandParameters());

                object response = SendMessageAsync(message).Result;

                property.ProcessGetResponse(response);

                if (!property.Result)
                    throw new InvalidOperationException(property.ResultMessage);

                result = property.Value;

                return true;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            try
            {
                var property = Properties.FirstOrDefault(p => p.Name == binder.Name);

                if (property == null)
                    return false;

                var parameters = new CommandParameters();

                parameters.Add("value", value);

                TerminalMessage message = property.GetSetMessage(parameters);

                object response = SendMessageAsync(message).Result;

                property.ProcessSetResponse(response);

                if (!property.Result)
                    throw new InvalidOperationException(property.ResultMessage);

                return true;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            result = null;
            
            var method = Methods.FirstOrDefault(m => m.Name == _callStack.Peek());

            if (method == null)
                return false;

            var methodName = _callStack.Pop();

            return TryInvoke(methodName, binder.CallInfo.ArgumentNames, args, out result);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            return TryInvoke(binder.Name, binder.CallInfo.ArgumentNames, args, out result);
        }

        private bool TryInvoke(string methodName, IReadOnlyCollection<string> argNames, object[] args, out object result)
        {
            result = null;

            try
            {
                var method = Methods.FirstOrDefault(m => m.Name == methodName);

                if (method == null)
                    return false;

                var parameters = new CommandParameters();

                foreach (var parameter in argNames.Zip(args, (name, arg) => new {Name = name, Value = arg}))
                {
                    parameters.Add(parameter.Name, parameter.Value);
                }

                TerminalMessage message = method.GetMessage(parameters);

                object response = SendMessageAsync(message).Result;

                method.ProcessInvokeResponse(response);

                if (!method.Result)
                    throw new InvalidOperationException(method.ResultMessage);

                result = method.Result;

                return true;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public virtual ITerminalRequestHandler Successor { get; protected set; }

        public virtual object HandleRequest(object command)
        {
            var outerCommand = Activator.CreateInstance(CommandType);

            outerCommand.GetType().InvokeMember("Item",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, outerCommand, new[] { command });

            return outerCommand;
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
