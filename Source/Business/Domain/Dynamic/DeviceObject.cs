using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public abstract class DeviceObject : DynamicObject, ITerminalDevice
    {
        protected DeviceObject()
        {
            Name = GetType().Name;
        }

        protected DeviceObject(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public Type CommandType { get; set; }
        public Type ResponseType { get; set; }

        public ITerminalDeviceMethodCollection Methods { get; } = new TerminalDeviceMethodCollection();
        public ITerminalDevicePropertyCollection Properties { get; } = new TerminalDevicePropertyCollection();

        public virtual Type SuccessorType { get; set; }

        public virtual object HandleRequest(object command)
        {
            var outerCommand = Activator.CreateInstance(CommandType);

            outerCommand.GetType().InvokeMember("Item",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, outerCommand, new[] { command });

            return outerCommand;
        }

        public virtual object HandleResponse(object response)
        {
            if (!(response is BaseResponse))
                throw new InvalidOperationException($"Expected {ResponseType.Name} in response handler");

            return response;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object value)
        {
            value = this;

            TerminalMessage message;

            var property = Properties.FirstOrDefault(p => p.Name == binder.Name);

            if (property == null)
            {
                var method = Methods.FirstOrDefault(m => m.Name == binder.Name);

                if (method == null)
                    return false;

                message = method.GetMessage();
            }
            else
            {
                message = property.GetGetMessage();
            }

            if (message == null)
                return false;

            var terminalClient = ServiceLocator.Current.GetInstance<ITerminalClient>();

            terminalClient.SendMessage(message);

            // todo wait for value

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var property = Properties.FirstOrDefault(p => p.Name == binder.Name);

            if (property == null)
                return false;

            property.Value = value;

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;

            var method = Methods.FirstOrDefault(m => m.Name == binder.Name);

            if (method == null)
                return false;

            TerminalMessage message = method.GetMessage();

            var terminalClient = ServiceLocator.Current.GetInstance<ITerminalClient>();

            terminalClient.SendMessage(message);

            // todo wait for result (success)
            result = true;

            return true;
        }
    }

    public class DeviceObjectT<TCommand, TResponse> : DeviceObject
    {
        public DeviceObjectT()
        {
            CommandType = typeof(TCommand);
            ResponseType = typeof(TResponse);
        }
    }

}
