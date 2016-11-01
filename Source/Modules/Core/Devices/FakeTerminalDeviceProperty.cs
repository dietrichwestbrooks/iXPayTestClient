using System;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Devices
{
    public class FakeTerminalDeviceProperty : TerminalDeviceMember, ITerminalDeviceProperty
    {
        public FakeTerminalDeviceProperty(string name) 
            : base(name)
        {
        }

        public ITerminalDeviceCommand GetCommand { get; }

        public ITerminalDeviceCommand SetCommand { get; }

        public PropertyInvoke InvokeFlag { get; set; } = PropertyInvoke.None;

        public string ValuePropertyName { get; }

        public bool TryGet(CommandParameters parameters, out object result)
        {
            result = null;

            return false;
        }

        public bool TrySet(CommandParameters parameters)
        {
            return false;
        }

        public static TerminalDeviceProperty Register<TValue, TGetCommand, TGetResponse, TSetCommand, TSetResponse>(
            string name, string valuePropName, Type ownerType, Func<TGetCommand> prepGetCmdFunc, Func<TSetCommand> prepSetCmdFunc)
        {
            return null;
        }
    }
}
