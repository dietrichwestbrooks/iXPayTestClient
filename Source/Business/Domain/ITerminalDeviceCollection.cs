using System;
using System.Collections.Generic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDeviceCollection : IEnumerable<ITerminalDevice>
    {
        ITerminalDevice this[string name] { get; }
        event EventHandler<ITerminalDevice> DeviceAdded;
    }
}
