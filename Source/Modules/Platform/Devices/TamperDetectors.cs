using System.Collections.Generic;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class TamperDetectors : TerminalDevice<TamperDetectorsCommand, TamperDetectorsResponse, TamperDetectorsEvent>
    {
        public TamperDetectors() 
            : base("TamperDetectors")
        {
            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new AvailableDetectorsProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new ConfirmDetectorOpenMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new StatusChangedEvent(this),
                });
        }

        #region Device Properties

        [ValueProperty("TamperDetector")]
        public class AvailableDetectorsProperty :
            TerminalDeviceProperty<TamperDetectors[], GetAvailableDetectorsCommand, GetAvailableDetectorsResponse>
        {
            public AvailableDetectorsProperty(ITerminalDevice device) 
                : base(device, "AvailableDetectors")
            {
                GetCommand = new TerminalDeviceCommand<GetAvailableDetectorsCommand, GetAvailableDetectorsResponse>(
                    this,
                    $"get_{Name}"
                    );
            }
        }

        #endregion

        #region Device Methods

        public class ConfirmDetectorOpenMethod :
            TerminalDeviceMethod<ConfirmDetectorOpenCommand, ConfirmDetectorOpenResponse>
        {
            public ConfirmDetectorOpenMethod(ITerminalDevice device)
                : base(device, "ConfirmDetectorOpen")
            {
                InvokeCommand = new TerminalDeviceCommand<ConfirmDetectorOpenCommand, ConfirmDetectorOpenResponse>(
                    this,
                    Name
                    );
            }
        }

        #endregion

        #region Device Events

        public class StatusChangedEvent : TerminalDeviceEvent<StatusChanged>
        {
            public StatusChangedEvent(ITerminalDevice device)
                : base(device, "StatusChanged")
            {
            }
        }

        #endregion
    }
}
