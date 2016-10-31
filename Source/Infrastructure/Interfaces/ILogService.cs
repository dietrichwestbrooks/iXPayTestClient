using Prism.Logging;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces
{
    public interface ILogService : ILoggerFacade
    {
        void AddTarget(ILogTarget target);
    }
}
