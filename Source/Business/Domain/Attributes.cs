using System;
using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TerminalDeviceAttribute : ExportAttribute
    {
        public TerminalDeviceAttribute()
            : base(typeof(ITerminalDevice))
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TerminalRequestHandlerAttribute : ExportAttribute
    {
        public TerminalRequestHandlerAttribute()
            : base(typeof(ITerminalRequestHandler))
        {
        }
    }

    //[AttributeUsage(AttributeTargets.Method)]
    //public class TerminalDeviceCommandAttribute : Attribute
    //{
    //    public TerminalDeviceCommandAttribute(Type command, Type response)
    //    {
    //        Command = command;
    //        Response = response;
    //    }

    //    public Type Command { get; set; }
    //    public Type Response { get; set; }
    //    public Type Successor { get; set; }
    //}

    //[AttributeUsage(AttributeTargets.Property)]
    //public class TerminalDevicePropertyAttribute : Attribute
    //{
    //    public TerminalDevicePropertyAttribute(Type getCommand, Type getResponse)
    //    {
    //        GetCommand = getCommand;
    //        GetResponse = getResponse;
    //    }

    //    public TerminalDevicePropertyAttribute(Type getCommand, Type getResponse, Type setCommand, Type setResponse)
    //    {
    //        GetCommand = getCommand;
    //        GetResponse = getResponse;
    //        SetCommand = setCommand;
    //        SetResponse = setResponse;
    //    }

    //    public Type GetCommand { get; set; }
    //    public Type GetResponse { get; set; }
    //    public Type SetCommand { get; set; }
    //    public Type SetResponse { get; set; }
    //    public Type Successor { get; set; }
    //}

    //[AttributeUsage(AttributeTargets.Event)]
    //public class TerminalDeviceEventAttribute : Attribute
    //{
    //    public TerminalDeviceEventAttribute(Type @event)
    //    {
    //        Event = @event;
    //    }

    //    public Type Event { get; set; }
    //}
}
