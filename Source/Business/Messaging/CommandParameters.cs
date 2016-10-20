using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public class CommandParameters : IEnumerable<KeyValuePair<string, object>>
    {
        private Dictionary<string, object> _parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public CommandParameters()
        {
            
        }

        public CommandParameters(IReadOnlyCollection<string> argNames, object[] args)
        {
            var unamedArgs = args.Take(args.Length - argNames.Count);

            foreach (var arg in unamedArgs)
            {
                Add(string.Empty, arg);
            }

            var namedArgs = args.Skip(args.Length - argNames.Count);

            // todo change to work with unnamed parameters as well
            foreach (var parameter in argNames.Zip(namedArgs, (name, arg) => new {Name = name, Value = arg}))
            {
                Add(parameter.Name, parameter.Value);
            }
        }

        public void Add<T>(string paramName, T value)
        {
            _parameters[paramName] = value;
        }

        public void Remove(string paramName)
        {
            _parameters.Remove(paramName);
        }

        public T GetValue<T>(string paramName, int paramOrder)
        {
            return Has(paramName) ? (T)_parameters[paramName] : default(T);
        }

        public T[] GetArray<T>(string paramName, int paramOrder)
        {
            return Has(paramName) ? (T[])_parameters[paramName] : default(T[]);
        }

        public bool Has(string paramName)
        {
            return _parameters.ContainsKey(paramName);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
