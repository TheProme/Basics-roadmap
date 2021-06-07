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

        public ThreadPool()
        {
            _poolWorker = new Thread(CheckJobsQueue) { IsBackground = true, Name = "ThreadPoolWorker"};
            _poolWorker.Start();
        }

        public void AddWorker()
        {
            lock(_locker)
            {
                QueueWorker newWorker = new QueueWorker($"worker{_count}");
                newWorker.NotifyCompletion += NewWorker_NotifyCompletionHandler;
                _pendingWorkers.Enqueue(newWorker);
                Console.WriteLine($"Added {newWorker.Name}");
                _count++;
            }
        }

        private void NewWorker_NotifyCompletionHandler(QueueWorker worker)
        {
            lock(_locker)
            {
                if (worker.HasFinishedTask)
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
            while(true)
            {
                if(_jobs.Count > 0 && _pendingWorkers.Count > 0)
                {
                    lock (_locker)
                    {
                        Console.WriteLine($"Tasks left: {_jobs.Count}");
                        var pickedJob = _jobs.Dequeue();
                        var pickedWorker = _pendingWorkers.Dequeue();
                        Console.WriteLine($"{pickedWorker.Name} was given task");
                        _activeWorkers.Add(pickedWorker);
                        pickedWorker.Execute(pickedJob);
                    }
                }
            }
        }
    }
}
