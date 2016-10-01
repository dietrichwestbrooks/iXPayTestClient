using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class DeviceCommand : IDeviceCommand
    {
        public DeviceCommand(string name, Type commandType)
        {
            Name = name;
            CommandType = commandType;
        }

        public DeviceCommand()
        {
            throw new NotImplementedException();
        }

        public string Name { get; }

        public Type CommandType { get; }
    }
}
