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
    public class ChipCardReader : TerminalDeviceT<ChipCardReaderCommand, ChipCardReaderResponse>
    {
        public ChipCardReader()
        {
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Subscribe(OnModulesInitialized);

            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new OpenChipCardReaderMethod {Successor = this},
                });
        }

        public void OnModulesInitialized()
        {
            var terminal = ServiceLocator.Current.GetInstance<ITerminalClientService>();
            Successor = terminal.Devices["Terminal"];
        }
    }

    public class OpenChipCardReaderMethod : MethodCommandT<OpenChipCardReaderCommand, OpenChipCardReaderResponse>
    {
        public OpenChipCardReaderMethod()
            : base("Open")
        {
            CreateInvokeCommand = p => InternalCreateCommand(
                p.GetValue<bool>("allowMagStripeFallback", 0),
                p.GetValue<bool>("allowMagStripeFallbackSpecified", 0),
                p.GetValue("timeout", 0, -1),
                p.GetValue<bool>("timeoutSpecified", 1));
        }

        private OpenChipCardReaderCommand InternalCreateCommand(bool allowMagStripeFallback,
            bool allowMagStripeFallbackSpecified, int timeout, bool timeoutSpecified)
        {
            return new OpenChipCardReaderCommand
                {
                    AllowMagStripeFallback = allowMagStripeFallback,
                    AllowMagStripeFallbackSpecified = allowMagStripeFallbackSpecified,
                    Timeout = timeout,
                    TimeoutSpecified = timeoutSpecified,
                };
        }
    }
}
