using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
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

        #region Convenience Methods and Properties
        // Add methods and properties that make it more convienent to send commands
        // to this device from scripting environment

        #endregion
    }

    public class OpenChipCardReaderMethod : MethodCommandT<OpenChipCardReaderCommand, OpenChipCardReaderResponse>
    {
        public OpenChipCardReaderMethod()
            : base("Open")
        {
            CreateInvokeCommand = p => new OpenChipCardReaderCommand
                {
                    AllowMagStripeFallback = p.GetValue<bool>("allowMagStripeFallback", 0),
                    AllowMagStripeFallbackSpecified = p.GetValue<bool>("allowMagStripeFallbackSpecified", 1),
                    Timeout = p.GetValue("timeout", 2, -1),
                    TimeoutSpecified = p.GetValue<bool>("timeoutSpecified", 3)
                };
        }
    }
}
