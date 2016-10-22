using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Commands;
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
        private string _connectionMessage;
        private string _statusBarMessage;
        private IPEndPoint _endPoint;

        public ShellViewModel()
        {
            //IconSource = new BitmapImage(new Uri("pack://application:,,,/AssemblyName;component/App.ico"));

            IconSource = Imaging.CreateBitmapSourceFromHIcon(Properties.Resources.AppIcon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            ConnectionMessage = "Not Connected";

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

        public string ConnectionMessage
        {
            get { return _connectionMessage; }
            set { SetProperty(ref _connectionMessage, value); }
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
                    ConnectionMessage = $"Connecting to {args.EndPoint}...";
                    IsConnecting = true;
                    break;

                case ConnectionEventType.Connected:
                    IsConnecting = false;
                    IsConnected = true;
                    IsHeartBeating = true;

                    if (!Equals(args.EndPoint, _endPoint))
                    {
                        ApplicationCommands.ShowNotificationCommand.Execute(new NotificationParameter
                            {
                                Type = NotificationType.Succeeded,
                                Message = $"Connected to {args.EndPoint}"
                            });
                    }

                    _endPoint = args.EndPoint;
                    HostAddress = args.EndPoint?.ToString();
                    ConnectionMessage = $"Connected to: {HostAddress}";
                    break;

                case ConnectionEventType.Pulse:
                    IsHeartBeating = args.Pulse;
                    break;

                case ConnectionEventType.Disconnected:
                    HostAddress = string.Empty;
                    IsConnected = false;
                    ConnectionMessage = "Not Connected";
                    break;

                case ConnectionEventType.Failed:
                    IsConnecting = false;
                    IsConnected = false;
                    IsHeartBeating = false;

                    ApplicationCommands.ShowNotificationCommand.Execute(new NotificationParameter
                        {
                        Type = NotificationType.Failed,
                        Message = args.Exception.Message
                    });
                    ConnectionMessage = "Not Connected";
                    break;

                default:
                    IsConnecting = false;
                    break;
            }
        }

        public string StatusBarMessage
        {
            get { return _statusBarMessage; }
            set { SetProperty(ref _statusBarMessage, value); }
        }
    }
}
