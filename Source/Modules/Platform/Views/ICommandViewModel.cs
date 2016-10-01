﻿using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    public interface ICommandViewModel : IViewModel
    {
        CommandType CommandType { get; }
        ITerminalDeviceCommand Object { get; }
    }
}
