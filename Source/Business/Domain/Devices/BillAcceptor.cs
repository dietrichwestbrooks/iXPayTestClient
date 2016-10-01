using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Messaging;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Domain.Devices
{
    public class BillAcceptor : Device
    {
        public BillAcceptor()
        {
            Commands = new List<IDeviceCommand>
                {
                    new DeviceCommand("GetAcceptorIdCommand", typeof (GetAcceptorIdCommand)),
                    new DeviceCommand("GetStatusCommand", typeof (GetStatusCommand)),
                    new DeviceCommand("GetBankNoteStatusCommand", typeof (GetBankNoteStatusCommand)),
                    new DeviceCommand("GetSafeboxStatusCommand", typeof (GetSafeboxStatusCommand)),
                    new DeviceCommand("GetOpenedCommand", typeof (GetOpenedCommand)),
                    new DeviceCommand("GetBillAcceptorCapabilitiesCommand", typeof (GetBillAcceptorCapabilitiesCommand)),
                    new DeviceCommand("GetCurrentNoteCommand", typeof (GetCurrentNoteCommand)),
                    new DeviceCommand("OpenBillAcceptorCommand", typeof (OpenBillAcceptorCommand)),
                    new DeviceCommand("CloseBillAcceptorCommand", typeof (CloseBillAcceptorCommand)),
                    new DeviceCommand("CollectCommand", typeof (CollectCommand)),
                    new DeviceCommand("EjectCommand", typeof (EjectCommand)),
                    new DeviceCommand("NoteStateConfirmCommand", typeof (NoteStateConfirmCommand)),
                };
        }

        public override string Name { get; } = "Bill Acceptor";

        public override Type CommandType { get; } = typeof(BillAcceptorCommand);

        public override IList<IDeviceCommand> Commands { get; }
    }
}
