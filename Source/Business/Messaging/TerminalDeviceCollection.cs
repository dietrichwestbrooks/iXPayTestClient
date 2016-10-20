using System;
using System.Collections.Generic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public class TerminalDeviceCollection : TerminalObjectCollection<ITerminalDevice>
    {
        public TerminalDeviceCollection()
        {

        }

        public TerminalDeviceCollection(IEnumerable<ITerminalDevice> devices)
            : base(devices)
        {
        }

        public event EventHandler<ITerminalDevice> DeviceAdded;

        protected override void OnObjectAdded(ITerminalDevice device)
        {
            DeviceAdded?.Invoke(this, device);
        }
    }
}
