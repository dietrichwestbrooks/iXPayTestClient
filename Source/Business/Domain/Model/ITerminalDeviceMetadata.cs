using System;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Model
{
    public interface ITerminalDeviceMetadata
    {
        string DeviceName { get; }

        Type DeviceType { get; }
    }
}
