using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public interface IDevice
    {
        string Name { get; }

        Type CommandType { get; }

        IList<IDeviceCommand> Commands { get; }
    }
}
