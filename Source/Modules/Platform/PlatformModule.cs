using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using Prism.Logging;
using Prism.Mef.Modularity;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging.New;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Modules;
using Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform
{
    [ModuleExport("PlatformModule", typeof (Module), DependsOnModuleNames = new[] {"CoreModule", "ScriptModule" })]
    public class PlatformModule : ModuleBase
    {
        protected override void LoadModule()
        {
        }

        protected override void RegisterDevices()
        {
            try
            {
                SamReader.RegisterDeviceProxy();

                EventAggregator.GetEvent<DeviceRegisteredEvent>().Subscribe(OnDeviceRegistered);

                var terminalService = Container.GetExportedValue<ITerminalService>();

                RegisterDevicesFromScript();

                var terminal = terminalService.Devices.FirstOrDefault(d => d.Name == "Terminal");

                foreach (var device in terminalService.Devices.Where(d => d.Name != terminal?.Name))
                {
                    device.Successor = terminal;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Category.Exception, Priority.Medium);
            }
        }

        protected override void RegisterViews()
        {
            RegionManager.Regions[RegionNames.LeftDockRegion].Add(ServiceLocator.Current.GetInstance<IDeviceListView>());
            RegionManager.Regions[RegionNames.MainDockRegion].Add(ServiceLocator.Current.GetInstance<IMessageEditorView>());
            RegionManager.Regions[RegionNames.MesssageEditorRegion].Add(ServiceLocator.Current.GetInstance<ICommandEditorView>());
            RegionManager.Regions[RegionNames.BottomLeftDockRegion].Add(ServiceLocator.Current.GetInstance<IMessageListView>());
            RegionManager.Regions[RegionNames.BottomDockRegion].Add(ServiceLocator.Current.GetInstance<IResponseListView>());
            RegionManager.Regions[RegionNames.BottomRightDockRegion].Add(ServiceLocator.Current.GetInstance<IEventListView>());

            RegionManager.Regions[RegionNames.RightWindowCommandsRegion].Add(ServiceLocator.Current.GetInstance<RightTitlebarCommands>());
            RegionManager.Regions[RegionNames.FlyoutRegion].Add(ServiceLocator.Current.GetInstance<IConnectView>());
        }

        private void RegisterDevicesFromScript()
        {
            try
            {
                var terminalService = Container.GetExportedValue<ITerminalService>();
                terminalService.RegisterDeviceFromFile(Path.Combine(Directory.GetCurrentDirectory(), @"Scripts\Devices\Beeper.py"));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Category.Exception, Priority.Medium);
            }
        }

        private void OnDeviceRegistered(ITerminalDevice device)
        {
            Container.ComposeExportedValue(device);
        }
    }
}
