using System.ComponentModel.Composition;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using Prism.Mef.Modularity;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform
{
    [ModuleExport("PlatformModule", typeof (Module), DependsOnModuleNames = new[] {"CoreModule", "ScriptModule" })]
    public class PlatformModule : ModuleBase
    {
        protected override void LoadModule()
        {
        }

        protected override void RegisterServices()
        {
            //todo find a better way to preload devices or get successors other than module intialized
            ServiceLocator.Current.GetInstance<ITerminalService>().Devices.GetEnumerator();

            EventAggregator.GetEvent<DeviceRegisteredEvent>().Subscribe(OnDeviceRegistered);
        }

        protected override void RegisterViews()
        {
            // View Discovery example
            //RegionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof (ICommandPickerView));

            RegionManager.Regions[RegionNames.LeftDockRegion].Add(ServiceLocator.Current.GetInstance<IDeviceListView>());
            RegionManager.Regions[RegionNames.MainDockRegion].Add(ServiceLocator.Current.GetInstance<IMessageEditorView>());
            RegionManager.Regions[RegionNames.MesssageEditorRegion].Add(ServiceLocator.Current.GetInstance<ICommandEditorView>());
            RegionManager.Regions[RegionNames.BottomLeftDockRegion].Add(ServiceLocator.Current.GetInstance<IMessageListView>());
            RegionManager.Regions[RegionNames.BottomDockRegion].Add(ServiceLocator.Current.GetInstance<IResponseListView>());
            RegionManager.Regions[RegionNames.BottomRightDockRegion].Add(ServiceLocator.Current.GetInstance<IEventListView>());

            RegionManager.Regions[RegionNames.RightWindowCommandsRegion].Add(ServiceLocator.Current.GetInstance<RightTitlebarCommands>());
            RegionManager.Regions[RegionNames.FlyoutRegion].Add(ServiceLocator.Current.GetInstance<IConnectView>());
        }

        private void OnDeviceRegistered(ITerminalDevice device)
        {
            Container.ComposeExportedValue(device);
        }
    }
}
