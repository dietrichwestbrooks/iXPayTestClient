using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using Prism.Mef.Modularity;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Modules;
using Wayne.Payment.Tools.iXPayTestClient.Modules.Core.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Core
{
    [ModuleExport("CoreModule", typeof(Module))]
    public class CoreModule : ModuleBase
    {
        protected override void RegisterViews()
        {
            RegionManager.Regions[RegionNames.RightWindowCommandsRegion].Add(ServiceLocator.Current.GetInstance<RightTitlebarCommands>());
            RegionManager.Regions[RegionNames.FlyoutRegion].Add(ServiceLocator.Current.GetInstance<ISettingsView>());
            RegionManager.Regions[RegionNames.FlyoutRegion].Add(ServiceLocator.Current.GetInstance<INotificationView>());
            RegionManager.Regions[RegionNames.RightDockRegion].Add(ServiceLocator.Current.GetInstance<IOutputViewerView>());

            RegionManager.RequestNavigate(RegionNames.OutputRegion, NavigateViewNames.LogView);
        }
    }
}
