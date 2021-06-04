using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolSync
{
    public class ThreadPool
    {
        private List<Func<bool>> Actions = new List<Func<bool>>();


        private List<QueueWorker> _threads = new List<QueueWorker>();

        public ThreadPool(int threadsCount)
        {
            for (int i = 0; i < threadsCount; i++)
            {
                _threads.Add(new QueueWorker(ref Actions, $"worker{i}"));
            }
        }

        public void AddActions(Func<bool> action, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Actions.Add(action);
            }
        }
    }
}
