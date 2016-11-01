using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.Extensions;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Devices
{
    public class FakeDeviceCommand<TCommand, TResponse> : TerminalDeviceCommand<TCommand, TResponse>
        where TCommand : BaseCommand, new()
        where TResponse : BaseResponse, new()
    {
        private readonly int _delay;

        public FakeDeviceCommand(ITerminalDeviceMember member, string name) 
            : base(member, name)
        {
            _delay = new Random().Next(1000, 5000);
        }

        public FakeDeviceCommand(ITerminalDeviceMember member, string name, Func<TCommand> prepareCommand = null) 
            : base(member, name, prepareCommand)
        {
            _delay = new Random().Next(1000, 5000);
        }

        protected override Task<object> SendMessageAsync(TerminalMessage message)
        {
            int sequentNumber = FakeSequenceNumberGenerator.Instance.Increment();

            message.SetCommandSequenceNumber(sequentNumber);

            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MessageSentEvent>().Publish(message);

            return Task.Run<object>(() =>
            {
                Thread.Sleep(_delay);

                BaseResponse response = new TResponse
                {
                    SequenceNumber = sequentNumber,
                    Success = true,
                    Message = "OK",
                };

                eventAggregator.GetEvent<ResponseReceivedEvent>().Publish(new TerminalMessage
                {
                    Item = new TerminalResponse
                    {
                        Item = new FakeDeviceResponse
                        {
                            Item = response
                        }
                    }
                });

                return response;
            });
        }
    }
}
