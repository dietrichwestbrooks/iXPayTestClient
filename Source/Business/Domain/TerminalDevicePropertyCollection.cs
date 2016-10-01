using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public class TerminalDevicePropertyCollection : ITerminalDevicePropertyCollection
    {
        private List<ITerminalDeviceProperty> _properties = new List<ITerminalDeviceProperty>();

        public ITerminalDeviceProperty this[string name]
        {
            get
            {
                var property = _properties.FirstOrDefault(p => p.Name == name);

                if (property == null)
                    throw new IndexOutOfRangeException(name);

                return property;
            }
        }

        public event EventHandler<ITerminalDeviceProperty> PropertyAdded;

        public void AddRange(IEnumerable<ITerminalDeviceProperty> properties)
        {
            if (properties == null)
                return;

            foreach (var property in properties)
            {
                Add(property);
            }
        }

        public void Add(ITerminalDeviceProperty property)
        {
            if (property == null)
                return;

            _properties.Add(property);
            OnPropertyAdded(property);
        }

        public IEnumerator<ITerminalDeviceProperty> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnPropertyAdded(ITerminalDeviceProperty property)
        {
            PropertyAdded?.Invoke(property, property);
        }
    }
}
