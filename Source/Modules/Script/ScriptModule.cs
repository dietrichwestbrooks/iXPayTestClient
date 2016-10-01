using System.Reflection;
using iXPayTestClient.Modules.Script.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Mef.Modularity;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;

namespace iXPayTestClient.Modules.Script
{
    [ModuleExport("ScriptModule", typeof(Module), DependsOnModuleNames = new[] { "CoreModule" })]
    public class ScriptModule : ModuleBase
    {
        protected override void ConfigureContainer()
        {
        }

        protected override void RegisterViews()
        {
            RegionManager.Regions[RegionNames.MainDockRegion].Add(ServiceLocator.Current.GetInstance<IScriptView>());
        }
    }
}
