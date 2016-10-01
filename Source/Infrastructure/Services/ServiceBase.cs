using System;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Prism.Logging;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Services
{
    public abstract class ServiceBase
    {
        protected ServiceBase()
        {
            EventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            if (EventAggregator == null)
                throw new InvalidOperationException("Unable to locate Event Aggregator");

            Logger = ServiceLocator.Current.GetInstance<ILoggerFacade>();

            if (Logger == null)
                throw new InvalidOperationException("Unable to locate Logger Service");
        }

        #region Properties

        protected IEventAggregator EventAggregator { get; set; }

        protected ILoggerFacade Logger { get; set; }

        #endregion
    }
}
