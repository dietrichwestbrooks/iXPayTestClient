using System;
using System.Dynamic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public sealed class TerminalDeviceMethod : TerminalDeviceMember, ITerminalDeviceMethod
    {
        public TerminalDeviceMethod(string name, Type commandType, Type responseType, Func<object> prepareCommandFunc)
            : base(name)
        {
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

        public Type CommandType { get; }

        public Type ResponseType { get; }

        public ITerminalDeviceCommand InvokeCommand { get; }

        public bool TryInvoke(CommandParameters parameters, out object result)
        {
            result = InvokeCommand.Execute(parameters);
            return true;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return TryInvoke(new CommandParameters(binder.CallInfo.ArgumentNames, args), out result);
        }
    }
}
