using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class ContactlessReader : Device
    {
        public ContactlessReader()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetStatusCommand", typeof (GetStatusCommand)),
                    new DeviceCommand("GetOpenedCommand", typeof (GetOpenedCommand)),
                    new DeviceCommand("GetContactlessReaderCapabilitiesCommand", typeof (GetContactlessReaderCapabilitiesCommand)),
                    new DeviceCommand("GetContactlessOperationalStateCommand", typeof (GetContactlessOperationalStateCommand)),
                    new DeviceCommand("GetGlobalEMVDataCommand", typeof (GetGlobalEMVDataCommand)),
                    new DeviceCommand("SetGlobalEMVDataCommand", typeof (SetGlobalEMVDataCommand)),
                    new DeviceCommand("SetBrandTerminalCapabilitiesCommand", typeof (SetBrandTerminalCapabilitiesCommand)),
                    new DeviceCommand("GetBrandTerminalCapabilitiesCommand", typeof (GetBrandTerminalCapabilitiesCommand)),
                    new DeviceCommand("GetBrandModeCommand", typeof (GetBrandModeCommand)),
                    new DeviceCommand("SetBrandModeCommand", typeof (SetBrandModeCommand)),
                    new DeviceCommand("GetContactlessApplicationCommand", typeof (GetContactlessApplicationCommand)),
                    new DeviceCommand("DownloadContactlessApplicationCommand", typeof (DownloadContactlessApplicationCommand)),
                    new DeviceCommand("DownloadContactlessPublicKeysCommand", typeof (DownloadContactlessPublicKeysCommand)),
                    new DeviceCommand("DeleteApplicationPublicKeysCommand", typeof (DeleteApplicationPublicKeysCommand)),
                    new DeviceCommand("SetBrandApplicationParametersCommand", typeof (SetBrandApplicationParametersCommand)),
                    new DeviceCommand("GetBrandApplicationParametersCommand", typeof (GetBrandApplicationParametersCommand)),
                    new DeviceCommand("DeleteContactlessApplicationCommand", typeof (DeleteContactlessApplicationCommand)),
                    new DeviceCommand("SetTransactionIndicatorCommand", typeof (SetTransactionIndicatorCommand)),
                    new DeviceCommand("CloseContactlessReaderCommand", typeof (CloseContactlessReaderCommand)),
                    new DeviceCommand("OpenContactlessReaderCommand", typeof (OpenContactlessReaderCommand)),
                    new DeviceCommand("ResetContactlessConfigurationCommand", typeof (ResetContactlessConfigurationCommand)),
                };
        }

        public override string Name { get; } = "Contactless Reader";

        public override Type CommandType { get; } = typeof(ContactlessReaderCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
