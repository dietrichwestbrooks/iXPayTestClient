using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public class TerminalDeviceCollection : ITerminalDeviceCollection
    {
        public TerminalDeviceCollection(IEnumerable<ITerminalDevice> devices)
        {
            AddRange(devices);
        }

        public TerminalDeviceCollection(IEnumerable<ITerminalDevice> devices, out Action<ITerminalDevice> addDevice)
        {
            addDevice = Add;

            AddRange(devices);
        }

        private List<ITerminalDevice> _devices = new List<ITerminalDevice>();

        public event EventHandler<ITerminalDevice> DeviceAdded;

        public ITerminalDevice this[string name]
        {
            get
            {
                var device = _devices.FirstOrDefault(d => d.Name == name);

                if (device == null)
                    throw new IndexOutOfRangeException(name);

                return device;
            }
        }

        public IEnumerator<ITerminalDevice> GetEnumerator()
        {
            return _devices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void OnDeviceAdded(ITerminalDevice device)
        {
            DeviceAdded?.Invoke(this, device);
        }

        private void AddRange(IEnumerable<ITerminalDevice> devices)
        {
            if (devices == null)
                return;

            foreach (var device in devices)
            {
                Add(device);
            }
        }

        private void Add(ITerminalDevice device)
        {
            if (device == null)
                return;

            _devices.Add(device);
            OnDeviceAdded(device);
        }
    }
}
