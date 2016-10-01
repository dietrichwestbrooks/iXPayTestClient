using System;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views
{
    public abstract class ViewModelBase : BindableBase
    {
        protected ViewModelBase()
        {
            EventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            if (EventAggregator == null)
                throw new InvalidOperationException("Unable to locate Event Aggregator");

            RegionManager = ServiceLocator.Current.GetInstance<IRegionManager>();

            if (RegionManager == null)
                throw new InvalidOperationException("Unable to locate Region Manager");

            Logger = ServiceLocator.Current.GetInstance<ILoggerFacade>();

            if (Logger == null)
                throw new InvalidOperationException("Unable to locate Logger Service");
        }

        #region Properties

        protected IEventAggregator EventAggregator { get; set; }

        protected IRegionManager RegionManager { get; set; }

        protected ILoggerFacade Logger { get; set; }

        #endregion

    }
}
