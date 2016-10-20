using System.Diagnostics;
using System.IO;
using System.Reflection;
using Wayne.Payment.Tools.iXPayTestClient.Modules.Script.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Mef.Modularity;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;

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
            var view = ServiceLocator.Current.GetInstance<IScriptEditorView>();
            RegionManager.Regions[RegionNames.MainDockRegion].Add(view);

            var parameters = new NavigationParameters
                {
                    {"SelectedTarget", true},
                };

            RegionManager.RequestNavigate(RegionNames.ScriptEditorRegion, "ScriptFileView", parameters);
        }
    }
}
