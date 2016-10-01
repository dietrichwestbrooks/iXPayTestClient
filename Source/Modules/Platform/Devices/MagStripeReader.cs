using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class MagStripeReader : TerminalDeviceT<MagStripeReaderCommand, MagStripeReaderResponse>
    {
        public MagStripeReader()
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new OpenMagStripeReaderMethod {Successor = this},
                });
        }

        public void OnModulesInitialized()
        {
            var terminal = ServiceLocator.Current.GetInstance<ITerminalClientService>();
            Successor = terminal.Devices["Terminal"];
        }
    }

    public class OpenMagStripeReaderMethod : MethodCommandT<OpenMagStripeReaderCommand, OpenMagStripeReaderResponse>
    {
        public OpenMagStripeReaderMethod()
             : base("Open")
        {
            CreateInvokeCommand = p => InternalCreateCommand(
                p.GetValue("timeout", 0, -1),
                p.GetValue<bool>("timeoutSpecified", 1));
        }

        private OpenMagStripeReaderCommand InternalCreateCommand(int timeout, bool timeoutSpecified)
        {
            return new OpenMagStripeReaderCommand
            {
                Timeout = timeout,
                TimeoutSpecified = timeoutSpecified,
            };
        }
    }
}
