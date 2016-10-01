using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using Prism.Mef.Modularity;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Modules.Test.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Test
{
    [ModuleExport("TestModule", typeof(Module), DependsOnModuleNames = new[] { "CoreModule" })]
    public class TestModule : ModuleBase
    {
        protected override void RegisterViews()
        {
            RegionManager.Regions[RegionNames.MainDockRegion].Add(ServiceLocator.Current.GetInstance<ITestListView>());
        }
    }
}
