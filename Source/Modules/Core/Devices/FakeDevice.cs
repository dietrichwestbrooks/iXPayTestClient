using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Devices
{
    [TerminalDevice]
    public class FakeDevice : TerminalDevice<FakeDeviceCommand, FakeDeviceResponse, FakeDeviceEvent>
    {
        public FakeDevice()
            : base("FakeDevice")
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);

            Properties.AddRange(new List<ITerminalDeviceProperty>
                {
                    new IterationsProperty(this),
                });

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new SimulateMethod(this),
                });

            Events.AddRange(new List<ITerminalDeviceEvent>
                {
                    new SimulateCompletedEvent(this),
                });
        }

        public void SimulateX(int iterations)
        {
            ((dynamic) this).Simulate(iterations);
        }

        private void OnModulesInitialized()
        {
            var terminal = ServiceLocator.Current.GetInstance<ITerminalService>();
            Successor = terminal.Devices["Terminal"];
        }
    }

    public class FakeDevicePropertyValues
    {
        public static int Iterations = 1;
        public static int Multiplier = 1;
    }

    #region Properties

    [ValueProperty("Iterations")]
    public class IterationsProperty : TerminalDeviceProperty<int, GetIterationsCommand, GetIterationsResponse,
    SetIterationsCommand, SetIterationsResponse>
    {
        public IterationsProperty(ITerminalDevice device)
            : base(device, "Iterations")
        {
            GetCommand = new FakeDeviceCommand<GetIterationsCommand, GetIterationsResponse>(
                this,
                $"get_{Name}"
                );

            SetCommand = new FakeDeviceCommand<SetIterationsCommand, SetIterationsResponse>(
                this,
                $"set_{Name}",
                () => new SetIterationsCommand
                    {
                        Iterations = 1,
                        Multiplier = 1,
                    }
                );
        }

        protected override bool TrySet(CommandParameters parameters, out object response)
        {
            bool retVal = base.TrySet(parameters, out response);

            if (parameters.Has("Iterations"))
                FakeDevicePropertyValues.Iterations = parameters.GetValue<int>("Iterations", 0);

            if (parameters.Has("Multiplier"))
                FakeDevicePropertyValues.Multiplier = parameters.GetValue<int>("Multiplier", 1);

            return retVal;
        }

        public override bool TryGet(CommandParameters parameters, out object result)
        {
            var retVal = base.TryGet(parameters, out result);

            var response = result as GetIterationsResponse;
            if (response != null)
            {
                response.Iterations = FakeDevicePropertyValues.Iterations;
                response.Multiplier = FakeDevicePropertyValues.Multiplier;
            }
            else
            {
                result = (Value = FakeDevicePropertyValues.Iterations);
            }

            return retVal;
        }
    }

    #endregion

    #region Methods

    public class SimulateMethod : TerminalDeviceMethod<SimulateCommand, SimulateResponse>
    {
        public SimulateMethod(ITerminalDevice device)
            : base(device, "Simulate")
        {
            InvokeCommand = new FakeDeviceCommand<SimulateCommand, SimulateResponse>(
                this,
                Name
                );
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            result = null;

            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            int iterations = FakeDevicePropertyValues.Multiplier*FakeDevicePropertyValues.Iterations;

            for (int i = 0; i < iterations; i++)
            {
                base.TryInvoke(binder, args, out result);

                new Thread(o =>
                {
                    Thread.Sleep(500);

                    eventAggregator.GetEvent<EventReceivedEvent>().Publish(new TerminalMessage
                    {
                        Item = new TerminalEvent
                        {
                            Item = new FakeDeviceEvent
                            {
                                Item = new SimulateCompleted
                                {
                                    Iteration = (int)o,
                                }
                            }
                        }
                    });

                    Device.Events.Single(e => e.Name == "SimulateCompleted").TryInvoke(new SimulateCompleted
                    {
                        Iteration = (int)o,
                    });
                }).Start(i + 1);
            }

            return true;
        }
    }

    #endregion

    #region Events

    public class SimulateCompletedEvent : TerminalDeviceEvent<SimulateCompleted>
    {
        public SimulateCompletedEvent(ITerminalDevice device)
            : base(device, "SimulateCompleted")
        {
        }
    }

    #endregion

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
