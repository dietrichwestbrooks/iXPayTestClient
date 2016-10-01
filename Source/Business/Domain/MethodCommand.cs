using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public abstract class MethodCommand : CommandObject, ITerminalDeviceMethod
    {
        protected MethodCommand(string name)
            : base(name)
        {
        }

        protected Type CommandType { get; set; }
        protected Type ResponseType { get; set; }
        protected virtual Func<CommandParameters, BaseCommand> CreateInvokeCommand { get; set; }
        //protected virtual Func<BaseResponse, bool> ProcessResponse { get; set; }

        public TerminalMessage GetMessage()
        {
            return GetMessage(new CommandParameters());
        }

        public virtual TerminalMessage GetMessage(CommandParameters parameters)
        {
            return GetMessage(CreateInvokeCommand(parameters));
        }

        public virtual bool ProcessInvokeResponse(object response)
        {
            return ProcessResponse(response);
        }
    }

    public abstract class MethodCommandT<TInvokeCommand, TInvokeResponse> : MethodCommand
        where TInvokeCommand : class
        where TInvokeResponse : class
    {
        protected MethodCommandT(string name)
            : base(name)
        {
            CommandType = typeof(TInvokeCommand);
            ResponseType = typeof(TInvokeResponse);
        }

        public new Func<CommandParameters, TInvokeCommand> CreateInvokeCommand { get; set; }
        //public new Func<TResponse, bool> ProcessResponse { get; set; }

        public override TerminalMessage GetMessage(CommandParameters parameters)
        {
            return GetMessage(CreateInvokeCommand(parameters));
        }

        public virtual bool ProcessInvokeResponse(TInvokeResponse response)
        {
            return base.ProcessInvokeResponse(response);
        }
    }
}
