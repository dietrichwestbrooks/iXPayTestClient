using System;
using System.Collections.Generic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public class TerminalDeviceEventCollection : TerminalObjectCollection<ITerminalDeviceEvent>
    {
        public TerminalDeviceEventCollection()
        {
            
        }

        public TerminalDeviceEventCollection(IEnumerable<ITerminalDeviceEvent> events) 
            : base(events)
        {
        }

        public event EventHandler<ITerminalDeviceEvent> EventAdded;

        protected override void OnObjectAdded(ITerminalDeviceEvent @event)
        {
            EventAdded?.Invoke(this, @event);
        }
    }
}
