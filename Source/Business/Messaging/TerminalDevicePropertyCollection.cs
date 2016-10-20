using System;
using System.Collections.Generic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public class TerminalDevicePropertyCollection : TerminalObjectCollection<ITerminalDeviceProperty>
    {
        public TerminalDevicePropertyCollection()
        {

        }

        public TerminalDevicePropertyCollection(IEnumerable<ITerminalDeviceProperty> properties)
            : base(properties)
        {
        }

        public event EventHandler<ITerminalDeviceProperty> PropertyAdded;

        protected override void OnObjectAdded(ITerminalDeviceProperty property)
        {
            PropertyAdded?.Invoke(this, property);
        }
    }
}
