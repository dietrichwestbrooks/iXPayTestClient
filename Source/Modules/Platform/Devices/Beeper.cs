using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class Beeper : TerminalDevice<BeeperCommand, BeeperResponse>, IPartImportsSatisfiedNotification
    {
        public Beeper() 
            : base("Beeper")
        {
            Methods.AddRange(new List<ITerminalDeviceMethod>
                {
                    new BeeperBeepMethod(this),
                });
        }

        public void OnImportsSatisfied()
        {
            var terminalService = ServiceLocator.Current.GetInstance<ITerminalService>();
            Successor = terminalService.Devices["Terminal"];
        }
    }

    public class BeeperBeepMethod : TerminalDeviceMethod<BeepCommand, BeeperResponse>
    {
        public BeeperBeepMethod(ITerminalDevice device) 
            : base(device, "Beep")
        {
            InvokeCommand = new TerminalDeviceCommand<BeepCommand, BeeperResponse>(
                this,
                Name,
                () => new BeepCommand
                    {
                        OnTime = 1000,
                        OnTimeSpecified = true,
                        OffTime = 100,
                        OffTimeSpecified = true,
                    }
                );
        }
    }
}
