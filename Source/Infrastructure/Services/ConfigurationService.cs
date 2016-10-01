using System.Configuration;
using System.ComponentModel.Composition;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;

namespace Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Services
{
    [Export(typeof(IConfigurationService))]
    public class ConfigurationService : ServiceBase, IConfigurationService
    {
        private Configuration _config;

        public ConfigurationService()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
        public string HostAddress
        {
            get { return _config.AppSettings.Settings["hostAddress"].Value; }
            set { _config.AppSettings.Settings["hostAddress"].Value = value; }
        }

        public int HostPort
        {
            get
            {
                int hostPort;

                string value = _config.AppSettings.Settings["hostPort"].Value;

                if (!int.TryParse(value, out hostPort))
                    hostPort = 0;

                return hostPort;
            }
            set { _config.AppSettings.Settings["hostPort"].Value = value.ToString(); }
        }

        public void Save()
        {
            _config.Save();
        }
    }
}
