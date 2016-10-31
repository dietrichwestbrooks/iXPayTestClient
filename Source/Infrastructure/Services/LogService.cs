using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using NLog;
using Prism.Logging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Services
{
    public class LogService : ILogService
    {
        private List<ILogTarget> _targets = new List<ILogTarget>();

        public LogService()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        private Logger Logger { get; }

        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    Logger.Debug(message);
                    break;

                case Category.Exception:
                    Logger.Error(message);
                    break;

                case Category.Info:
                    Logger.Info(message);
                    break;

                case Category.Warn:
                    Logger.Warn(message);
                    break;

                default:
                    Logger.Info(message);
                    break;
            }

            foreach (var target in _targets)
            {
                target.Log(message, category, priority);
            }
        }

        public void AddTarget(ILogTarget target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            _targets.Add(target);
        }
    }
}
