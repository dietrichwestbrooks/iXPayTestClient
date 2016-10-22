using System;
using System.IO;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using Prism.Logging;
using Prism.Mef.Modularity;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Modules.Emv.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Emv
{
    [ModuleExport("EmvModule", typeof(Module), DependsOnModuleNames = new[] { "CoreModule", "PlatformModule" })]
    public class EmvModule : ModuleBase
    {
        protected override void RegisterViews()
        {
            RegionManager.Regions[RegionNames.RightDockRegion].Add(ServiceLocator.Current.GetInstance<ITransactionView>());
        }

        protected override void RegisterDevices()
        {
            try
            {
                var terminalService = Container.GetExportedValue<ITerminalService>();

                if (terminalService == null)
                    throw new InvalidOperationException("Unable to locate terminal service");

                //terminalService.RegisterDeviceFromFile(Path.Combine(Directory.GetCurrentDirectory(), @"Scripts\EmvModule.py"));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Category.Exception, Priority.Medium);
            }
        }
    }
}
