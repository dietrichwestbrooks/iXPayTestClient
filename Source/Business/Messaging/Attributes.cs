using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
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

}
