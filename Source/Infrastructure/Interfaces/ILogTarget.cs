using Prism.Logging;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces
{
    public interface ILogTarget
    {
        void Log(string message, Category category, Priority priority);
    }
}
