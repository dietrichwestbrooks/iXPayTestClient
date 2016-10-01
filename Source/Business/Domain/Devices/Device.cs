using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public abstract class Device : IDevice
    {
        public abstract string Name { get; }

        public abstract Type CommandType { get; }

        public abstract IList<IDeviceCommand> Commands { get; }
    }
}
