using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public sealed class TerminalDeviceProperty : TerminalDeviceMember, ITerminalDeviceProperty
    {
        private TerminalDeviceProperty(string name, string valuePropName, Type valueType, Type getCommandType,
            Type getResponseType, Func<object> prepGetCmdFunc, Type setCommandType, Type setResponseType,
            Func<object> prepSetCmdFunc)
            : base(name)
        {
            ValuePropertyName = valuePropName;
            ValueType = valueType;
            GetCommandType = getCommandType;
            GetResponseType = getResponseType;
            GetCommand = new TerminalDeviceCommand(this, $"get_{Name}", GetCommandType, GetResponseType, prepGetCmdFunc);
            SetCommandType = setCommandType;
            SetResponseType = setResponseType;
            SetCommand = new TerminalDeviceCommand(this, $"set_{Name}", SetCommandType, SetResponseType, prepSetCmdFunc);
        }

        private TerminalDeviceProperty(string name, string valuePropName, Type valueType, Type getCommandType,
            Type getResponseType, Func<object> prepGetCmdFunc)
            : base(name)
        {
            ValuePropertyName = valuePropName;
            ValueType = valueType;
            GetCommandType = getCommandType;
            GetResponseType = getResponseType;
            GetCommand = new TerminalDeviceCommand(this, $"get_{Name}", GetCommandType, GetResponseType, prepGetCmdFunc);
        }

        public static TerminalDeviceProperty Register<TValue, TGetCommand, TGetResponse, TSetCommand, TSetResponse>(
            string name, string valuePropName, Type ownerType, Func<TGetCommand> prepGetCmdFunc = null,
            Func<TSetCommand> prepSetCmdFunc = null)
            where TGetCommand : class
            where TGetResponse : class
            where TSetCommand : class
            where TSetResponse : class
        {
            var property = new TerminalDeviceProperty(name, valuePropName, typeof (TValue), typeof (TGetCommand),
                typeof (TGetResponse),
                prepGetCmdFunc, typeof (TSetCommand), typeof (TSetResponse), prepSetCmdFunc);

            RegisterCommon(ownerType, property);

            return property;
        }

        public static TerminalDeviceProperty Register<TValue, TGetCommand, TGetResponse>(string name,
            string valuePropName, Type ownerType, Func<TGetCommand> prepGetCmdFunc = null)
            where TGetCommand : class
            where TGetResponse : class
        {
            var property = new TerminalDeviceProperty(name, valuePropName, typeof (TValue), typeof (TGetCommand),
                typeof (TGetResponse),
                prepGetCmdFunc);

            RegisterCommon(ownerType, property);

            return property;
        }

        private static void RegisterCommon(Type ownerType, TerminalDeviceProperty property)
        {
            TerminalDevice.AddMember(ownerType, property);
        }

        public string ValuePropertyName { get; }

        public Type ValueType { get; }

        public Type GetCommandType { get; }

        public Type GetResponseType { get; }

        public Type SetCommandType { get; }

        public Type SetResponseType { get; }

        public ITerminalDeviceCommand GetCommand { get; }

        public ITerminalDeviceCommand SetCommand { get; }

        public PropertyInvoke InvokeFlag { get; set; }

        public bool TryGet(CommandParameters parameters, out object result)
        {
            var response = GetCommand.Execute(parameters);

            object value = GetValue(response);

            result = InvokeFlag == PropertyInvoke.Get ? response : value;

            return true;
        }

        private object GetValue(object response)
        {
            PropertyInfo valueProperty = response.GetType().GetProperty(ValuePropertyName);
            return valueProperty?.GetValue(response);
        }

        public bool TrySet(CommandParameters parameters)
        {
            object response;
            TrySet(parameters, out response);
            return true;
        }

        private bool TrySet(CommandParameters parameters, out object result)
        {
            SetValue(parameters);
            result = SetCommand.Execute(parameters);
            return true;
        }

        private void SetValue(CommandParameters parameters)
        {
            var value = parameters.Where(p => p.Key.ToLower() == "value").Select(p => p.Value).FirstOrDefault();

            parameters.Remove("value");

            if (value == null)
                return;

            PropertyInfo valueProperty = SetCommand.GetType().GetProperty(ValuePropertyName);

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

    [Flags]
    public enum PropertyInvoke
    {
        None = 0,
        Get = 1,
        Set = 2,
    }
}
