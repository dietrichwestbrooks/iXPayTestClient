using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Devices
{
    public class FakeDevice
    {
        public static void RegisterDeviceProxy()
        {
            TerminalDevice.Register<FakeDeviceCommand, FakeDeviceResponse, FakeDeviceEvent>(
                    "FakeDevice", new TerminalRequestHandlerByName("Terminal"), typeof(FakeDevice));
        }

        #region Device Properties

        public static readonly TerminalDeviceProperty StatusProperty =
            FakeTerminalDeviceProperty.Register<int, GetIterationsCommand, GetIterationsResponse,
                SetIterationsCommand, SetIterationsResponse>("Iterations",
                    "Iterations", typeof (FakeDevice), null, () => new SetIterationsCommand
                        {
                            Iterations = 1,
                            Multiplier = 1,
                        });

        #endregion

        #region Device Methods

        public static readonly TerminalDeviceMethod OpenMethod =
            FakeTerminalDeviceMethod.Register<SimulateCommand, SimulateResponse>("Simulate",
                typeof (FakeDevice));

        #endregion

        #region Device Events

        public static readonly TerminalDeviceEvent SimulateCompletedEvent =
            TerminalDeviceEvent.Register<SimulateCompleted>("SimulateCompleted", typeof(FakeDevice));

        #endregion
    }

    #region TerminalCommands

    public class FakeDeviceCommand
    {
        public object Item { get; set; }
    }

    public class FakeDeviceResponse
    {
        public object Item { get; set; }
    }

    public class FakeDeviceEvent
    {
        public object Item { get; set; }
    }

    public class SimulateCompleted
    {
        public int Iteration { get; set; }
    }

    public class SimulateCommand : BaseCommand
    {
    }

    public class SimulateResponse : BaseResponse
    {
    }

    public class GetIterationsCommand : BaseCommand
    {
    }

    public class GetIterationsResponse : BaseResponse
    {
        public int Iterations { get; set; }

        public int Multiplier { get; set; }
    }

    public class SetIterationsCommand : BaseCommand
    {
        public int Iterations { get; set; }

        public int Multiplier { get; set; }
    }

    public class SetIterationsResponse : BaseResponse
    {
    } 

    #endregion
}
