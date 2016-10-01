using System;
using System.Collections.Generic;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain
{
    public class CommandParameters
    {
        private Dictionary<string, object> _parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public void Add<T>(string paramName, T value)
        {
            _parameters[paramName] = value;
        }

        public T GetValue<T>(string paramName, int paramOrder, T defaultValue = default(T))
        {
            return Has(paramName) ? (T)_parameters[paramName] : defaultValue;
        }

        public T[] GetArray<T>(string paramName, int paramOrder, T[] defaultValue = null)
        {
            return default(T[]);
        }

        public bool Has(string paramName)
        {
            return _parameters.ContainsKey(paramName);
        }
    }
}
