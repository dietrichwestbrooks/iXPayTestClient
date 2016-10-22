using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public abstract class TerminalDeviceProperty<TValue, TGetCommand, TGetResponse> : TerminalDeviceMember, ITerminalDeviceProperty
        where TGetCommand : class
        where TGetResponse : class
    {
        protected TerminalDeviceProperty(ITerminalDevice device, string name)
            : base(device, name)
        {
            ValueType = typeof (TValue);
            GetCommandType = typeof (TGetCommand);
            GetResponseType = typeof (TGetResponse);
        }

        public Type ValueType { get; }
        public Type GetCommandType { get; }
        public Type GetResponseType { get; }

        public TValue Value { get; set; }

        public ITerminalDeviceCommand GetCommand { get; protected set; }

        public ITerminalDeviceCommand SetCommand { get; protected set; }

        public PropertyInvoke InvokeFlag { get; set; }

        public virtual bool TryGet(CommandParameters parameters, out object result)
        {
            var response = GetCommand.Execute(parameters);

            Value = GetValue(response);

            result = InvokeFlag == PropertyInvoke.Get ? response : Value;

            return true;
        }

        private TValue GetValue(object response)
        {
            PropertyInfo valueProperty = null;

            var attribute = GetType().GetCustomAttributes(typeof (ValuePropertyAttribute), false).FirstOrDefault() as ValuePropertyAttribute;

            if (attribute != null)
            {
                valueProperty = response.GetType()
                        .GetProperties()
                        .FirstOrDefault(p =>
                                string.Equals(p.Name, attribute.PropertyName, StringComparison.CurrentCultureIgnoreCase) &&
                                p.PropertyType == typeof(TValue));
            }

            if (valueProperty == null)
            {
                valueProperty = response.GetType()
                        .GetProperties()
                        .FirstOrDefault(p => p.Name.ToLower() == "value" && p.PropertyType == typeof(TValue));
            }

            if (valueProperty == null)
            {
                valueProperty = response.GetType()
                        .GetProperties()
                        .FirstOrDefault(p => p.PropertyType == typeof(TValue));
            }

            return (TValue)valueProperty?.GetValue(response);
        }

        public bool TrySet(CommandParameters parameters)
        {
            object response;
            TrySet(parameters, out response);
            return true;
        }

        protected virtual bool TrySet(CommandParameters parameters, out object result)
        {
            SetValue(parameters);
            result = SetCommand.Execute(parameters);
            return true;
        }

        private void SetValue(CommandParameters parameters)
        {
            PropertyInfo valueProperty = null;

            var value = parameters.Where(p => p.Key.ToLower() == "value").Select(p => p.Value).FirstOrDefault();

            parameters.Remove("value");

            if (value == null)
                return;

            var attribute = GetType().GetCustomAttributes(typeof(ValuePropertyAttribute), false).FirstOrDefault() as ValuePropertyAttribute;

            if (attribute != null)
            {
                valueProperty = SetCommand.CommandType
                        .GetProperties()
                        .FirstOrDefault(p =>
                                string.Equals(p.Name, attribute.PropertyName, StringComparison.CurrentCultureIgnoreCase) &&
                                p.PropertyType == typeof(TValue));
            }

            if (valueProperty == null)
            {
                valueProperty = SetCommand.CommandType
                        .GetProperties()
                        .FirstOrDefault(p => p.Name.ToLower() == "value" && p.PropertyType == typeof(TValue));
            }

            if (valueProperty == null)
            {
                valueProperty = SetCommand.CommandType
                        .GetProperties()
                        .FirstOrDefault(p => p.PropertyType == typeof(TValue));
            }

            if (valueProperty == null)
                return;

            parameters.Add(valueProperty.Name, value);
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            result = null;

            bool success = false;

            try
            {
                switch (InvokeFlag)
                {
                    case PropertyInvoke.Get:
                        success = TryGet(new CommandParameters(binder.CallInfo.ArgumentNames, args), out result);
                        break;

                    case PropertyInvoke.Set:
                        success = TrySet(new CommandParameters(binder.CallInfo.ArgumentNames, args), out result);
                        break;
                }
            }
            finally
            {
                InvokeFlag = PropertyInvoke.None;
            }

            return success;
        }
    }

    public abstract class TerminalDeviceProperty<TValue, TGetCommand, TGetResponse, TSetCommand, TSetResponse> :
        TerminalDeviceProperty<TValue, TGetCommand, TGetResponse>
        where TGetCommand : class
        where TGetResponse : class
        where TSetCommand : class
        where TSetResponse : class
    {
        protected TerminalDeviceProperty(ITerminalDevice device, string name)
            : base(device, name)
        {
            SetCommandType = typeof (TSetCommand);
            SetResponseType = typeof (TSetResponse);
        }

        public Type SetCommandType { get; }
        public Type SetResponseType { get; }
    }

    [Flags]
    public enum PropertyInvoke
    {
        None = 0,
        Get = 1,
        Set = 2,
    }
}
