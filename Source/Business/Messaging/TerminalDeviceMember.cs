using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public abstract class TerminalDeviceMember : DynamicObject
    {
        protected TerminalDeviceMember(ITerminalDevice device, string name)
        {
            Device = device;
            Name = name;
        }

        public ITerminalDevice Device { get; set; }

        public string Name { get; }
    }
}
