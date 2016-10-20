using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Wayne.Payment.Tools.iXPayTestClient.Business.Messaging
{
    public class ProducerConsumerQueue : IDisposable
    {
        private bool _disposed;
        private BlockingCollection<Task> _messageQueue = new BlockingCollection<Task>();

        public ProducerConsumerQueue(int workerCount)
        {
            for (int i = 0; i < workerCount; i++)
            {
                Task.Factory.StartNew(Consume);
            }
        }

        ~ProducerConsumerQueue()
        {
            Dispose(false);
        }

        public Task Enqueue(Action action, CancellationToken cancelToken = default(CancellationToken))
        {
            var task = new Task(action);
            _messageQueue.Add(task, cancelToken);
            return task;
        }

        private void Consume()
        {
            foreach (var task in _messageQueue.GetConsumingEnumerable())
            {
                try
                {
                    if (!task.IsCanceled)
                        task.RunSynchronously();
                }
                catch (InvalidOperationException) { }
            }
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
                _messageQueue.CompleteAdding();
            }

            _disposed = true;
        }
    }
}
