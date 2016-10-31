using System;
using System.Threading;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    internal class TerminalConnectionLocker : IDisposable
    {
        private bool _disposed;
        private bool _owner;
        private Mutex _mutex;
        private bool _hasHandle;

        public TerminalConnectionLocker(bool owner = false)
        {
            _owner = owner;

            if (_owner)
            {
                _mutex = new Mutex(false, GetType().FullName);
            }
            else
            {
                _mutex = Mutex.OpenExisting(GetType().FullName);
                _hasHandle = _mutex.WaitOne();
            }
        }

        ~TerminalConnectionLocker()
        {
            Dispose(false);
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
                if (_owner)
                {
                    _mutex.Close();
                }
                else
                {
                    if (_hasHandle)
                        _mutex.ReleaseMutex();
                }
            }

            _disposed = true;
        }
    }
}
