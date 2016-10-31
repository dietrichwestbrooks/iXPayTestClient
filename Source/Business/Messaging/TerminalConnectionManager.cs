using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    [Export(typeof(ITerminalConnectionManager))]
    public class TerminalConnectionManager : ITerminalConnectionManager
    {
        private bool _disposed;
        private ITerminalConnection _connection;
        private TerminalConnectionLocker _connectionLocker;

        public TerminalConnectionManager()
        {
            _connectionLocker = new TerminalConnectionLocker(true);
        }

        ~TerminalConnectionManager()
        {
            Dispose(false);
        }

        public ITerminalConnection Connection
        {
            get { return _connection; }
            private set
            {
                var oldConnection = _connection;
                var newConnecion = value;

                if (oldConnection == newConnecion)
                    return;

                _connection = newConnecion;
                FireConnectionChanged(oldConnection, newConnecion);
            }
        }

        public event EventHandler<ActiveConnectionChangedArgs> ConnectionChanged;

        public Task ConnectAsync(IPEndPoint endPoint, bool isSecure, bool keepAlive)
        {
            var client = ServiceLocator.Current.GetInstance<ITerminalClient>();
            Connection = client;
            return client.ConnectAsync(endPoint, isSecure, keepAlive);
        }

        public Task StartAsync(IPEndPoint endPoint, bool isSecure)
        {
            var server = ServiceLocator.Current.GetInstance<ITerminalServer>();
            Connection = server;
            return server.StartAsync(endPoint, isSecure);
        }

        private void FireConnectionChanged(ITerminalConnection oldConnection, ITerminalConnection newConnecion)
        {
            ConnectionChanged?.Invoke(this, new ActiveConnectionChangedArgs
                {
                    OldConnection = oldConnection,
                    NewConnection = newConnecion,
                });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _connectionLocker.Dispose();
            }

            _disposed = true;
        }
    }
}
