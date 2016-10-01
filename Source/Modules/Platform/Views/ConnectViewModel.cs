using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
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
        private string _hostAddress;
        private int _hostPort;
        private bool _isConnecting;
        private bool _isConnected;
        private IPEndPoint _ipEndPoint;

        public ConnectViewModel()
        {
            Terminal = ServiceLocator.Current.GetInstance<ITerminalClientService>();
            Configuration = ServiceLocator.Current.GetInstance<IConfigurationService>();

            HostAddress = Configuration.HostAddress;
            HostPort = Configuration.HostPort;

            ConnectCommand = new DelegateCommand<IPEndPoint>(OnConnect, CanConnect);
            DisconnectCommand = new DelegateCommand<IPEndPoint>(OnDisconnect, CanDisconnect);

            EventAggregator.GetEvent<ConnectionStatusEvent>().Subscribe(OnConnectionStatus);
        }

        public ICommand ConnectCommand { get; private set; }

        public ICommand DisconnectCommand { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string HostAddress
        {
            get { return _hostAddress; }
            set { SetProperty(ref _hostAddress, value); }
        }

        public int HostPort
        {
            get { return _hostPort; }
            set { SetProperty(ref _hostPort, value); }
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

        private ITerminalClientService Terminal { get; }

        private bool CanConnect(IPEndPoint endPoint)
        {
            return true;
            //todo Fix CommandParameter binding not firing CanExecute when values change return endPoint != null;
        }

        private void OnConnect(IPEndPoint endPoint)
        {
            ApplicationCommands.ShowFlyoutCommand.Execute(FlyoutNames.ConnectFlyout);

            Terminal.Connect(endPoint);
        }

        private bool CanDisconnect(IPEndPoint arg)
        {
            return IsConnected;
        }

        private void OnDisconnect(IPEndPoint endPoint)
        {
            Terminal.Disconnect(endPoint);
        }

        private void OnConnectionStatus(ConnectionEventArgs args)
        {
            switch (args.Type)
            {
                case ConnectionEventType.Connecting:
                    EndPoint = args.EndPoint;
                    IsConnecting = true;
                    break;

                case ConnectionEventType.Connected:
                    EndPoint = args.EndPoint;
                    IsConnecting = false;
                    IsConnected = true;
                    break;

                case ConnectionEventType.Disconnected:
                    EndPoint = args.EndPoint;
                    IsConnecting = false;
                    IsConnected = false;
                    break;

                default:
                    IsConnecting = false;
                    break;
            }
        }
    }
}
