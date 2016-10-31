using System.Reflection;
using Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Mef.Modularity;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Modules;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Script
{
    [ModuleExport("ScriptModule", typeof(Module), DependsOnModuleNames = new[] { "CoreModule" })]
    public class ScriptModule : ModuleBase
    {
        protected override void ConfigureContainer()
        {
        }

        protected override void RegisterViews()
        {
            var scriptEditorView = ServiceLocator.Current.GetInstance<IScriptEditorView>();
            RegionManager.Regions[RegionNames.MainDockRegion].Add(scriptEditorView);

            var parameters = new NavigationParameters
                {
                    {"SelectedTarget", true},
                };

            RegionManager.RequestNavigate(RegionNames.ScriptEditorRegion, NavigateViewNames.ScriptFileView, parameters);
            RegionManager.RequestNavigate(RegionNames.OutputRegion, NavigateViewNames.ScriptOutputView);
        }
    }
}
