using NLog;
using Prism.Logging;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Services
{
    public class LogService : ILoggerFacade
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    _logger.Debug(message);
                    break;

                case Category.Exception:
                    _logger.Error(message);
                    break;

                case Category.Info:
                    _logger.Info(message);
                    break;

                case Category.Warn:
                    _logger.Warn(message);
                    break;

                default:
                    _logger.Info(message);
                    break;
            }
        }
    }
}
