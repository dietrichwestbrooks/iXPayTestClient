﻿using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Devices
{
    [TerminalRequestHandler]
    [TerminalDevice]
    public class Terminal : TerminalDeviceT<TerminalCommand, TerminalResponse>
    {
    }
}
