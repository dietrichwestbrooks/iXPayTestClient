using System;
using System.Dynamic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public abstract class TerminalDeviceMethod<TInvokeCommand, TInvokeResponse> : TerminalDeviceMember,
        ITerminalDeviceMethod
        where TInvokeCommand : class
        where TInvokeResponse : class
    {
        protected TerminalDeviceMethod(ITerminalDevice device, string name)
            : base(device, name)
        {
            CommandType = typeof (TInvokeCommand);
            ResponseType = typeof (TInvokeResponse);
        }

        public Type CommandType { get; }
        public Type ResponseType { get; }

        public ITerminalDeviceCommand InvokeCommand { get; protected set; }

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
}
