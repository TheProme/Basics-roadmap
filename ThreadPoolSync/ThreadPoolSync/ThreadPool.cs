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
        private static int _count = 0;
        private readonly object _locker = new object();
        private Queue<Action> _jobs = new Queue<Action>();
        private Queue<QueueWorker> _pendingWorkers = new Queue<QueueWorker>();
        private List<QueueWorker> _activeWorkers = new List<QueueWorker>();

        private Thread _poolWorker;

        public ThreadPool(int numberOfWorkers)
        {
            _poolWorker = new Thread(CheckJobsQueue) { IsBackground = true, Name = "ThreadPoolWorker"};
            _poolWorker.Start();
            for (int i = 0; i < numberOfWorkers; i++)
            {
                AddWorker();
            }
        }

        protected void AddWorker()
        {

            QueueWorker newWorker = new QueueWorker($"worker{_count}");
            newWorker.NotifyCompletion += NewWorker_NotifyCompletionHandler;
            lock (_locker)
            {
                _pendingWorkers.Enqueue(newWorker);
            }
            Console.WriteLine($"Added {newWorker.Name}");
            _count++;
        }

        private void NewWorker_NotifyCompletionHandler(QueueWorker worker)
        {
            if (worker.CompletedTask)
            {
                lock(_locker)
                {
                    _activeWorkers.Remove(worker);
                    _pendingWorkers.Enqueue(worker);
                }
            }
        }

        public void AddJob(Action job)
        {
            lock(_locker)
            {
                _jobs.Enqueue(job);
            }
        }

        private void CheckJobsQueue()
        {
            while(_poolWorker.IsAlive)
            {
                if(_jobs.Count > 0 && _pendingWorkers.Count > 0)
                {
                    Console.WriteLine($"Tasks left: {_jobs.Count}");
                    var pickedJob = _jobs.Dequeue();
                    var pickedWorker = _pendingWorkers.Dequeue();
                    Console.WriteLine($"{pickedWorker.Name} was given task");
                    _activeWorkers.Add(pickedWorker);
                    pickedWorker.Execute(pickedJob);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
