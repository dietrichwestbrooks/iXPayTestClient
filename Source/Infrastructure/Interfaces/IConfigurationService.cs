using System.Net;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces
{
    public interface IConfigurationService
    {
        string HostAddress { get; set; }
        int HostPort { get; set; }
        void Save();
    }
}
