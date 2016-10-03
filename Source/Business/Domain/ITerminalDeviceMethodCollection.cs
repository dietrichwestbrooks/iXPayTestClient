using System;
using System.Collections.Generic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public interface ITerminalDeviceMethodCollection : IEnumerable<ITerminalDeviceMethod>
    {
        ITerminalDeviceMethod this[string name] { get; }
        void AddRange(IEnumerable<ITerminalDeviceMethod> methods);
        void Add(ITerminalDeviceMethod method);
        event EventHandler<ITerminalDeviceMethod> MethodAdded;
    }
}
