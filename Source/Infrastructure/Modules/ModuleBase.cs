using System;
using System.ComponentModel.Composition.Hosting;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Modules
{
    public abstract class ModuleBase : IModule
    {
        protected ModuleBase()
        {
            Container = ServiceLocator.Current.GetInstance<CompositionContainer>();

            if (Container == null)
                throw new InvalidOperationException("Unable to locate Composition Container");

            EventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            if (EventAggregator == null)
                throw new InvalidOperationException("Unable to locate Event Aggregator");

            RegionManager = ServiceLocator.Current.GetInstance<IRegionManager>();

            if (RegionManager == null)
                throw new InvalidOperationException("Unable to locate Region Manager");

            Logger = ServiceLocator.Current.GetInstance<ILogService>();

            if (Logger == null)
                throw new InvalidOperationException("Unable to locate Logger Service");

        }

        #region Properties

        protected IRegionManager RegionManager { get; set; }

        protected ILogService Logger { get; set; }

        protected CompositionContainer Container { get; set; }

        protected IEventAggregator EventAggregator { get; set; }

        #endregion

        public virtual void Initialize()
        {
            LoadModule();

            ConfigureContainer();

            RegisterServices();

            RegisterDevices();

            RegisterViews();
        }

        protected virtual void LoadModule()
        {
        }

        protected virtual void ConfigureContainer()
        {
        }

        protected virtual void RegisterServices()
        {
            
        }

        protected virtual void RegisterViews()
        {
        }

        protected virtual void RegisterDevices()
        {
        }
    }
}
