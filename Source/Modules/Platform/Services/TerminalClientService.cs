using System;
using System.ComponentModel.Composition;
using System.Net;
using Microsoft.Practices.ServiceLocation;
using Prism.Logging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Domain;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Commands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Services;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Services
{
    [Export(typeof(ITerminalClientService))]
    public class TerminalClientService : ServiceBase, ITerminalClientService
    {
        private Action<ITerminalDevice> _addDevice;
        private ITerminalDeviceCollection _devices;

        public TerminalClientService()
        {
            RegisterScriptVariables();

            Client = ServiceLocator.Current.GetInstance<ITerminalClient>();
            Configuration = ServiceLocator.Current.GetInstance<IConfigurationService>();

            Client.Connected += OnClientConnected;
            Client.Disconnected += OnClientDisconnected;
            Client.Error += OnClientError;
            Client.MessageSent += OnClientMessageSent;
            Client.EventReceived += OnClientEventReceived;
            Client.Pulse += OnClientPulse;
            Client.ResponseReceived += OnClientResponseReceived;
        }

        public bool IsConnected => Client.IsConnected;

        public ITerminalDeviceCollection Devices => GetDevices();

        public void Connect(IPEndPoint endPoint)
        {
            Client.ConnectAsync(endPoint);

            EventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionEventArgs
                {
                    Type = ConnectionEventType.Connecting,
                    EndPoint = endPoint
                });
        }

        public void Disconnect(IPEndPoint endPoint)
        {
            Client.Disconnect();

            EventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionEventArgs
            {
                Type = ConnectionEventType.Disconnecting,
                EndPoint = endPoint
            });
        }

        public void RegisterDevice(ITerminalDevice device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            // todo better implementation
            GetDevices();

            _addDevice(device);

            EventAggregator.GetEvent<DeviceRegisteredEvent>().Publish(device);
        }

        public ITerminalDevice RegisterDeviceFromFile(string path)
        {
            var device = ScriptService.CreateDeviceFromFile(path);

            RegisterDevice(device);

            return device;
        }

        private void OnClientResponseReceived(object sender, TerminalMessage message)
        {
            EventAggregator.GetEvent<ResponseReceivedEvent>().Publish(message);
        }

        private void OnClientPulse(object sender, PulseEventArgs args)
        {
            EventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionEventArgs
            {
                Type = ConnectionEventType.Pulse,
                EndPoint = args.EndPoint,
                Pulse = args.Pulse
            });
        }

        private void OnClientEventReceived(object sender, TerminalMessage message)
        {
            EventAggregator.GetEvent<EventReceivedEvent>().Publish(message);
        }

        private void OnClientConnected(object sender, IPEndPoint endPoint)
        {
            Configuration.HostAddress = endPoint.Address.ToString();
            Configuration.HostPort = endPoint.Port;
            Configuration.Save();

            ApplicationCommands.ShowNotificationCommand.Execute(new NotificationParameter()
            {
                Type = NotificationType.Succeeded,
                Message = $"Connected to {endPoint}"
            });

            EventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionEventArgs
            {
                Type = ConnectionEventType.Connected,
                EndPoint = endPoint,
                Pulse = true
            });
        }

        private void OnClientDisconnected(object sender, IPEndPoint endPoint)
        {
            EventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionEventArgs
            {
                Type = ConnectionEventType.Disconnected,
                EndPoint = endPoint,
                Pulse = false
            });
        }

        private void OnClientMessageSent(object sender, TerminalMessage message)
        {
            EventAggregator.GetEvent<MessageSentEvent>().Publish(message);
        }

        private void OnClientError(object sender, ClientErrorEventArg args)
        {
            switch (args.ErrorType)
            {
                case Enums.ClientErrorType.ConnectionError:
                    ApplicationCommands.ShowNotificationCommand.Execute(new NotificationParameter()
                    {
                        Type = NotificationType.Failed,
                        Message = args.Exception.Message
                    });

                    EventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionEventArgs
                    {
                        Type = ConnectionEventType.Failed,
                        EndPoint = args.EndPoint,
                        Exception = args.Exception,
                        Pulse = false
                    });
                    Logger.Log(args.Exception.Message, Category.Exception, Priority.High);
                    break;

                default:
                    Logger.Log(args.Exception.Message, Category.Exception, Priority.High);
                    break;
            }
        }

        private ITerminalClient Client { get; }

        private IConfigurationService Configuration { get; set; }

        private ITerminalDeviceCollection GetDevices()
        {
            return _devices ??
                   (_devices =
                       new TerminalDeviceCollection(ServiceLocator.Current.GetAllInstances<ITerminalDevice>(),
                           out _addDevice));
        }

        private IScriptService ScriptService
        {
            get
            {
                var scriptService = ServiceLocator.Current.GetInstance<IScriptService>();

                if (scriptService == null)
                    throw new InvalidOperationException("Unalble to locate script service");

                return scriptService;
            }
        }

        private void RegisterScriptVariables()
        {
            ScriptService.SetVariable("Terminal", this, true);
        }
    }
}
