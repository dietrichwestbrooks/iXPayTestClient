using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public class TerminalDeviceMethodCollection : ITerminalDeviceMethodCollection
    {
        private List<ITerminalDeviceMethod> _methods = new List<ITerminalDeviceMethod>();

        public event EventHandler<ITerminalDeviceMethod> MethodAdded;

        public ITerminalDeviceMethod this[string name]
        {
            get
            {
                var method = _methods.FirstOrDefault(p => p.Name == name);

                if (method == null)
                    throw new IndexOutOfRangeException(name);

                return method;
            }
        }

        public void AddRange(IEnumerable<ITerminalDeviceMethod> methods)
        {
            if (methods == null)
                return;

            foreach (var method in methods)
            {
                Add(method);
            }
        }

        public void Add(ITerminalDeviceMethod method)
        {
            if (method == null)
                return;

            _methods.Add(method);
            OnMethodAdded(method);
        }

        public IEnumerator<ITerminalDeviceMethod> GetEnumerator()
        {
            return _methods.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnMethodAdded(ITerminalDeviceMethod method)
        {
            MethodAdded?.Invoke(this, method);
        }
    }
}
