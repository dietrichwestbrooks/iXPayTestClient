using System;
using System.ComponentModel.Composition;
using System.Net;
using Microsoft.Practices.ServiceLocation;
using Prism.Logging;
using Wayne.Payment.Tools.iXPayTestClient.Business.Messaging;
using Wayne.Payment.Tools.iXPayTestClient.Business.TerminalCommands;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Events;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Interfaces;
using Wayne.Payment.Tools.iXPayTestClient.Infrastructure.Services;

namespace Wayne.Payment.Tools.iXPayTestClient.Modules.Platform.Services
{
    [Export(typeof(ITerminalService))]
    public class TerminalService : ServiceBase, ITerminalService
    {
        private object _connectionLocker = new object();

        public TerminalService()
        {
            ScriptService = ServiceLocator.Current.GetInstance<IScriptService>();
            Configuration = ServiceLocator.Current.GetInstance<IConfigurationService>();

            ConnectionManager = ServiceLocator.Current.GetInstance<ITerminalConnectionManager>();
            ConnectionManager.ConnectionChanged += OnConnectionChanged;

            OnSetupScript(ScriptService);

            EventAggregator.GetEvent<SetupScriptEvent>().Subscribe(OnSetupScript);
            EventAggregator.GetEvent<TeardownScriptEvent>().Subscribe(OnTeardownScript);
        }

        public TerminalDeviceCollection Devices
            => new TerminalDeviceCollection(ServiceLocator.Current.GetAllInstances<ITerminalDevice>());

        public void Connect(IPEndPoint endPoint, bool isClient, bool keepAlive, bool isSecure)
        {
            if (isClient)
            {
                ConnectionManager.ConnectAsync(endPoint, isSecure, keepAlive);
                EventAggregator.GetEvent<ConnectingEvent>().Publish(endPoint);
            }
            else
            {
                ConnectionManager.StartAsync(endPoint, isSecure);
                EventAggregator.GetEvent<ListeningEvent>().Publish(endPoint);
            }
        }

        public void Disconnect(IPEndPoint endPoint)
        {
            lock (_connectionLocker)
            {
                if (Connection == null)
                    throw new InvalidOperationException("No Connection");

                Connection.Disconnect(); 
            }

            EventAggregator.GetEvent<DisconnectedEvent>().Publish(endPoint);
        }

        public void SendMessage(TerminalMessage message)
        {
            lock (_connectionLocker)
            {
                if (Connection == null)
                    throw new InvalidOperationException("No Connection");

                Connection.SendMessage(message); 
            }
        }

        public void RegisterDevice(ITerminalDevice device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            EventAggregator.GetEvent<DeviceRegisteredEvent>().Publish(device);
        }

        public ITerminalDevice RegisterDeviceFromFile(string path)
        {
            var device = ScriptService.CreateDeviceFromFile(path);

            RegisterDevice(device);

            return device;
        }

        private void OnResponseReceived(object sender, TerminalMessage message)
        {
            EventAggregator.GetEvent<ResponseReceivedEvent>().Publish(message);
        }

        private void OnPulse(object sender, IPEndPoint endPoint)
        {
            EventAggregator.GetEvent<PulseEvent>().Publish(endPoint);
        }

        private void OnEventReceived(object sender, TerminalMessage message)
        {
            EventAggregator.GetEvent<EventReceivedEvent>().Publish(message);
        }

        private void OnConnected(object sender, IPEndPoint endPoint)
        {
            Configuration.HostAddress = endPoint.Address.ToString();
            Configuration.HostPort = endPoint.Port;
            Configuration.Save();

            EventAggregator.GetEvent<ConnectedEvent>().Publish(endPoint);
        }

        private void OnDisconnected(object sender, IPEndPoint endPoint)
        {
            EventAggregator.GetEvent<DisconnectedEvent>().Publish(endPoint);
        }

        private void OnMessageSent(object sender, TerminalMessage message)
        {
            EventAggregator.GetEvent<MessageSentEvent>().Publish(message);
        }

        private void OnError(object sender, ConnectionErrorEventArgs args)
        {
            switch (args.ErrorType)
            {
                case ClientErrorType.ConnectionError:
                    EventAggregator.GetEvent<ConnectionErrorEvent>().Publish(args.Exception);
                    Logger.Log(args.Exception.Message, Category.Exception, Priority.High);
                    break;

                case ClientErrorType.DataSendError:
                    EventAggregator.GetEvent<DataSendErrorEvent>().Publish(args.Exception);
                    Logger.Log(args.Exception.Message, Category.Exception, Priority.High);
                    break;

                case ClientErrorType.DataReceiveError:
                    EventAggregator.GetEvent<DataReceiveErrorEvent>().Publish(args.Exception);
                    Logger.Log(args.Exception.Message, Category.Exception, Priority.High);
                    break;

                default:
                    Logger.Log(args.Exception.Message, Category.Exception, Priority.High);
                    break;
            }
        }

        private IConfigurationService Configuration { get; }

        private IScriptService ScriptService { get; }

        private ITerminalConnectionManager ConnectionManager { get; }

        private ITerminalConnection Connection { get; set; }

        private void OnTeardownScript(IScriptService scriptService)
        {
        }

        private void OnSetupScript(IScriptService scriptService)
        {
            foreach (var device in Devices)
            {
                device.ClearEventHandlers();
                scriptService.SetVariable(device.Name, device);
            }
        }

        private void OnConnectionChanged(object sender, ActiveConnectionChangedArgs e)
        {
            if (e.OldConnection != null)
            {
                e.OldConnection.Connected -= OnConnected;
                e.OldConnection.Disconnected -= OnDisconnected;
                e.OldConnection.Error -= OnError;
                e.OldConnection.MessageSent -= OnMessageSent;
                e.OldConnection.EventReceived -= OnEventReceived;
                e.OldConnection.Pulse -= OnPulse;
                e.OldConnection.ResponseReceived -= OnResponseReceived;
            }

            if (e.NewConnection != null)
            {
                e.NewConnection.Connected += OnConnected;
                e.NewConnection.Disconnected += OnDisconnected;
                e.NewConnection.Error += OnError;
                e.NewConnection.MessageSent += OnMessageSent;
                e.NewConnection.EventReceived += OnEventReceived;
                e.NewConnection.Pulse += OnPulse;
                e.NewConnection.ResponseReceived += OnResponseReceived;
            }

            lock (_connectionLocker) Connection = e.NewConnection;
        }
    }
}
