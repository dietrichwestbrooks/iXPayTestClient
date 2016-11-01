using System;
using System.Dynamic;
using System.Linq;
using System.Threading;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Devices
{
    public class FakeTerminalDeviceMethod : TerminalDeviceMember, ITerminalDeviceMethod
    {
        public FakeTerminalDeviceMethod(string name) 
            : base(name)
        {
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            result = null;

            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            int iterations = FakeDevicePropertyValues.Multiplier * FakeDevicePropertyValues.Iterations;

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

        public static TerminalDeviceMethod Register<TCommand, TResponse>(string simulate, Type ownerType)
        {
            return null;
        }

        public ITerminalDeviceCommand InvokeCommand { get; }

        public bool TryInvoke(CommandParameters parameters, out object result)
        {
            result = null;

            return false;
        }
    }
}
