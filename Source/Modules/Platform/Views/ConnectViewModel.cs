using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Constants;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Views;
using ApplicationCommands = Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Commands.ApplicationCommands;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Views
{
    [Export(typeof(IConnectViewModel))]
    public class ConnectViewModel : ViewModelBase, IConnectViewModel
    {
        private string _title = "Connection";
        private string _serverAddress;
        private int _serverPort;
        private bool _isConnecting;
        private bool _isConnected;
        private IPEndPoint _ipEndPoint;
        private bool _autoReconnect;
        private bool _isClient;
        private bool _isSecure;

        public ConnectViewModel()
        {
            TerminalService = ServiceLocator.Current.GetInstance<ITerminalService>();
            Configuration = ServiceLocator.Current.GetInstance<IConfigurationService>();

            ServerAddress = Configuration.HostAddress;
            ServerPort = Configuration.HostPort;
            AutoReconnect = true;
            IsSecure = false;
            IsClient = true;

            ConnectCommand = new DelegateCommand<IPEndPoint>(OnConnect, CanConnect);
            DisconnectCommand = new DelegateCommand<IPEndPoint>(OnDisconnect, CanDisconnect);

            EventAggregator.GetEvent<ConnectingEvent>().Subscribe(OnConnecting);
            EventAggregator.GetEvent<ConnectedEvent>().Subscribe(OnConnected);
            EventAggregator.GetEvent<ConnectionErrorEvent>().Subscribe(OnConnectionError);
            EventAggregator.GetEvent<DisconnectedEvent>().Subscribe(OnDisconnected);
        }

        public ICommand ConnectCommand { get; private set; }

        public ICommand DisconnectCommand { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string ServerAddress
        {
            get { return _serverAddress; }
            set { SetProperty(ref _serverAddress, value); }
        }

        public int ServerPort
        {
            get { return _serverPort; }
            set { SetProperty(ref _serverPort, value); }
        }

        public bool AutoReconnect
        {
            get { return _autoReconnect; }
            set { SetProperty(ref _autoReconnect, value); }
        }

        public bool IsSecure
        {
            get { return _isSecure; }
            set { SetProperty(ref _isSecure, value); }
        }

        public bool IsClient
        {
            get { return _isClient; }
            set { SetProperty(ref _isClient, value); }
        }

        public bool IsConnecting
        {
            get { return _isConnecting; }
            set { SetProperty(ref _isConnecting, value); }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set { SetProperty(ref _isConnected, value); }
        }

        public IPEndPoint EndPoint
        {
            get { return _ipEndPoint; }
            set { SetProperty(ref _ipEndPoint, value); }
        }

        private IConfigurationService Configuration { get; }

        private ITerminalService TerminalService { get; }

        private bool CanConnect(IPEndPoint endPoint)
        {
            return true;
            //todo Fix CommandParameter binding not firing CanExecute when values change return endPoint != null;
        }

        private void OnConnect(IPEndPoint endPoint)
        {
            ApplicationCommands.ShowFlyoutCommand.Execute(FlyoutNames.ConnectFlyout);

            TerminalService.Connect(endPoint, IsClient, AutoReconnect, IsSecure);
        }

        private bool CanDisconnect(IPEndPoint arg)
        {
            return IsConnected;
        }

        private void OnDisconnect(IPEndPoint endPoint)
        {
            TerminalService.Disconnect(endPoint);
        }

        private void OnConnecting(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
            IsConnecting = true;
        }

        private void OnConnected(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
            IsConnecting = false;
            IsConnected = true;
        }

        private void OnConnectionError(Exception exception)
        {
            IsConnected = false;
            IsConnecting = false;

            ApplicationCommands.ShowNotificationCommand.Execute(new NotificationParameter
            {
                Title = "Connection Failed",
                Type = NotificationType.Failed,
                Message = exception?.Message
            });
        }

        private void OnDisconnected(IPEndPoint endPoint)
        {
            IsConnected = false;
            IsConnecting = false;
            EndPoint = null;
        }
    }
}
