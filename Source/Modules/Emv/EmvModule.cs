using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using Prism.Mef.Modularity;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Modules;
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
    }
}
