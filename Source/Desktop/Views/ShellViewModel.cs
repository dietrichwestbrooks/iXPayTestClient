using System;
using System.ComponentModel.Composition;
using System.Net;
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
        private string _connectionMessage;
        private string _statusBarMessage;

        public ShellViewModel()
        {
            //IconSource = new BitmapImage(new Uri("pack://application:,,,/AssemblyName;component/App.ico"));

            IconSource = Imaging.CreateBitmapSourceFromHIcon(Properties.Resources.AppIcon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            ConnectionMessage = "Not Connected";

            EventAggregator.GetEvent<ConnectingEvent>().Subscribe(OnConnecting);
            EventAggregator.GetEvent<ConnectedEvent>().Subscribe(OnConnected);
            EventAggregator.GetEvent<ConnectionErrorEvent>().Subscribe(OnConnectionError);
            EventAggregator.GetEvent<DisconnectedEvent>().Subscribe(OnDisconnected);
            EventAggregator.GetEvent<PulseEvent>().Subscribe(OnPulse);
        }

        public ImageSource IconSource { get; set; }

        public string Title { get; set; } = "Test Client";

        public string HostAddress
        {
            get { return _hostAddress; }
            set { SetProperty(ref _hostAddress, value); }
        }

        public string StatusBarMessage
        {
            get { return _statusBarMessage; }
            set { SetProperty(ref _statusBarMessage, value); }
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

        private void OnConnecting(IPEndPoint endPoint)
        {
            ConnectionMessage = $"Connecting to {endPoint}...";
            IsConnecting = true;
        }

        private void OnConnected(IPEndPoint endPoint)
        {
            IsConnecting = false;
            IsConnected = true;
            IsHeartBeating = true;
            HostAddress = endPoint?.ToString();
            ConnectionMessage = $"Connected to: {HostAddress}";
        }

        private void OnConnectionError(Exception exception)
        {
            HostAddress = string.Empty;
            IsConnected = false;
            IsConnecting = false;
            IsHeartBeating = false;
            ConnectionMessage = "Not Connected";
        }

        private void OnDisconnected(IPEndPoint endPoint)
        {
            HostAddress = string.Empty;
            IsConnected = false;
            IsConnecting = false;
            IsHeartBeating = false;
            ConnectionMessage = "Not Connected";
        }

        private void OnPulse(IPEndPoint endPoint)
        {
            IsHeartBeating = true;
        }
    }
}
