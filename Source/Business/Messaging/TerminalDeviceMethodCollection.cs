using System;
using System.Collections.Generic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public class TerminalDeviceMethodCollection : TerminalObjectCollection<ITerminalDeviceMethod>
    {
        public TerminalDeviceMethodCollection()
        {

        }

        public TerminalDeviceMethodCollection(IEnumerable<ITerminalDeviceMethod> methods)
            : base(methods)
        {
        }

        public event EventHandler<ITerminalDeviceMethod> MethodAdded;

        protected override void OnObjectAdded(ITerminalDeviceMethod method)
        {
            MethodAdded?.Invoke(this, method);
        }
    }
}
