using System;
using System.Collections.Generic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public interface ITerminalDevicePropertyCollection : IEnumerable<ITerminalDeviceProperty>
    {
        ITerminalDeviceProperty this[string name] { get; }
        void AddRange(IEnumerable<ITerminalDeviceProperty> properties);
        void Add(ITerminalDeviceProperty property);
        event EventHandler<ITerminalDeviceProperty> PropertyAdded;
    }
}