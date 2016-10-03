using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public abstract class PropertyCommand : CommandObject, ITerminalDeviceProperty
    {
        protected PropertyCommand(string name)
            : base (name)
        {
        }

        protected Type GetCommandType { get; set; }
        protected Type GetResponseType { get; set; }
        protected Type SetCommandType { get; set; }
        protected Type SetResponseType { get; set; }
        protected virtual Func<CommandParameters, BaseCommand> CreateGetCommand { get; set; }
        //protected virtual Func<object, object> ProcessGetResponse { get; set; }
        protected virtual Func<CommandParameters, BaseCommand> CreateSetCommand { get; set; }
        //protected virtual Func<BaseResponse, bool> ProcessSetResponse { get; set; }

        public virtual object Value { get; set; }

        public virtual TerminalMessage GetGetMessage(CommandParameters parameters)
        {
            return GetMessage(CreateGetCommand(parameters));
        }

        public virtual TerminalMessage GetSetMessage(CommandParameters parameters)
        {
            return GetMessage(CreateSetCommand(parameters));
        }

        public virtual bool ProcessGetResponse(object response)
        {
            return ProcessResponse(response);
        }

        public virtual bool ProcessSetResponse(object response)
        {
            return ProcessResponse(response);
        }
    }

    public abstract class PropertyCommandT<TValue, TGetCommand, TGetResponse> : PropertyCommand
        where TGetCommand : BaseCommand
        where TGetResponse : class
    {
        protected PropertyCommandT(string name)
            : base(name)
        {
            GetCommandType = typeof(TGetCommand);
            GetResponseType = typeof(TGetResponse);
        }

        public new TValue Value { get; set; }

        protected new Func<CommandParameters, TGetCommand> CreateGetCommand { get; set; }
        //protected new Func<TGetResponse, TValue> ProcessGetResponse { get; set; }

        public override TerminalMessage GetGetMessage(CommandParameters parameters)
        {
            return GetMessage(CreateGetCommand(parameters));
        }

        public virtual bool ProcessGetResponse(TGetResponse response)
        {
            return base.ProcessGetResponse(response);
        }
    }

    public abstract class PropertyCommandT<TValue, TGetCommand, TGetResponse, TSetCommand, TSetResponse> : PropertyCommandT<TValue, TGetCommand, TGetResponse>
        where TGetCommand : BaseCommand
        where TGetResponse : class
        where TSetCommand : class
        where TSetResponse : BaseResponse
    {
        protected PropertyCommandT(string name)
            : base(name)
        {
            SetCommandType = typeof(TSetCommand);
            SetResponseType = typeof(TSetResponse);
        }

        protected new Func<CommandParameters, TSetCommand> CreateSetCommand { get; set; }
        //protected new Func<TSetResponse, bool> ProcessSetResponse { get; set; }

        public override TerminalMessage GetSetMessage(CommandParameters parameters)
        {
            return GetMessage(CreateSetCommand(parameters));
        }

        public virtual bool ProcessSetResponse(TSetResponse response)
        {
            return base.ProcessGetResponse(response);
        }
    }
}
