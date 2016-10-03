using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;

namespace Wayne.Payment.Tools.iXPayTestClient.Desktop.Views
{
    [Export(typeof(IShellViewModel))]
    public class ShellViewModel : ViewModelBase, IShellViewModel
    {
        private string _hostAddress;
        private bool _isHeartBeating;
        private bool _isConnected;
        private bool _isConnecting;
        private string _connectingMessage;

        public ShellViewModel()
        {
            //IconSource = new BitmapImage(new Uri("pack://application:,,,/AssemblyName;component/App.ico"));

            IconSource = Imaging.CreateBitmapSourceFromHIcon(Properties.Resources.AppIcon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            EventAggregator.GetEvent<ConnectionStatusEvent>().Subscribe(OnConnectionStatus);
        }

        public ImageSource IconSource { get; set; }

        public string Title { get; set; } = "Test Client";

        public string HostAddress
        {
            get { return _hostAddress; }
            set { SetProperty(ref _hostAddress, value); }
        }

        public bool IsHeartBeating
        {
            get { return _isHeartBeating; }
            set { SetProperty(ref _isHeartBeating, value); }
        }

        public string ConnectingMessage
        {
            get { return _connectingMessage; }
            set { SetProperty(ref _connectingMessage, value); }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set { SetProperty(ref _isConnected, value); }
        }

        public bool IsConnecting
        {
            get { return _isConnecting; }
            set { SetProperty(ref _isConnecting, value); }
        }

        private void OnConnectionStatus(ConnectionEventArgs args)
        {
            switch (args.Type)
            {
                case ConnectionEventType.Connecting:
                    ConnectingMessage = $"Connecting to {args.EndPoint}...";
                    IsConnecting = true;
                    break;

                case ConnectionEventType.Connected:
                    IsConnecting = false;
                    IsConnected = true;
                    IsHeartBeating = true;
                    HostAddress = args.EndPoint.ToString();
                    break;

                case ConnectionEventType.Pulse:
                    IsHeartBeating = args.Pulse;
                    break;

                case ConnectionEventType.Disconnected:
                    HostAddress = string.Empty;
                    IsConnected = false;
                    break;

                default:
                    IsConnecting = false;
                    break;
            }
        }
    }
}
