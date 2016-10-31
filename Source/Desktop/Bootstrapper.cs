using System.Windows;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Prism.Events;
using Prism.Logging;
using Prism.Mef;
using Prism.Modularity;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Desktop.Views;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Modules;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Services;
using Wayne.Payment.Tools.iXPayTestClient.Modules.Core;

namespace Wayne.Payment.Tools.iXPayTestClient.Desktop
{
    public class Bootstrapper : MefBootstrapper
    {
        public Bootstrapper()
        {
            //AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
            //{
            //    string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //    if (folderPath == null) return null;
            //    string assemblyPath = Path.Combine(folderPath, new AssemblyName(e.Name).Name + ".dll");
            //    if (!File.Exists(assemblyPath)) return null;
            //    Assembly assembly = Assembly.LoadFrom(assemblyPath);
            //    return assembly;
            //};
        }

        protected override DependencyObject CreateShell()
        {
            return Container.GetExportedValue<IShellView>() as Window;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }

        protected override CompositionContainer CreateContainer()
        {
            var container = base.CreateContainer();
            container.ComposeExportedValue(container);
            return container;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.ComposeExportedValue(Logger as ILogService);

            Container.GetExportedValues<IFlyoutService>();
        }

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(CoreModule).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(TerminalConnectionManager).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ModuleBase).Assembly));

            if (!Directory.Exists("Modules"))
                Directory.CreateDirectory("Modules");

            AggregateCatalog.Catalogs.Add(new DirectoryCatalog("Modules"));
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override ILoggerFacade CreateLogger()
        {
            return new LogService();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            var eventAggregator = Container.GetExportedValue<IEventAggregator>();
            eventAggregator.GetEvent<ModulesInitializedEvent>().Publish();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

            //mappings.RegisterMapping(typeof(LayoutAnchorGroup), Container.GetExportedValue<LayoutAnchorGroupRegionAdapter>());
            //mappings.RegisterMapping(typeof(LayoutDocumentPane), Container.GetExportedValue<LayoutDocumentPaneRegionAdapter>());
            //mappings.RegisterMapping(typeof(LayoutAnchorSide), Container.GetExportedValue<LayoutAnchorSideRegionAdapter>());

            return mappings;
        }
    }
}
