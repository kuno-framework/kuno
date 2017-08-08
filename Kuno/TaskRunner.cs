using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kuno.Services.Messaging;

namespace Kuno
{
    public class TaskRunner
    {
        private bool _shuttingDown;

        private readonly List<Task> _tasks = new List<Task>();

        public T Add<T>(Func<T> function) where T : Task
        {
            if (_shuttingDown)
            {
                throw new InvalidOperationException("The task runner is shutting down and cannot accept new messages.");
            }

            var task = function();
            Task.WhenAll(task).ContinueWith(e =>
            {
                _tasks.Remove(task);
            });
            this._tasks.Add(task);

            return task;
        }

        public Task Shutdown()
        {
            _shuttingDown = true;

            return Task.WhenAll(_tasks);
        }
    }
}
