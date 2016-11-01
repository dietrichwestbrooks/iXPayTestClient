using System.Dynamic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public abstract class TerminalDeviceMember : DynamicObject
    {
        protected TerminalDeviceMember(string name)
        {
            Name = name;
        }

        public ITerminalDevice Device { get; private set; }

        public string Name { get; }

        public void Attach(TerminalDevice device)
        {
            if (Device == device)
                return;

            Device = device;
        }
    }
}
