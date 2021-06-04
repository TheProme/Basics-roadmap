using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolSync
{
    public class QueueWorker : ISynchronisationContext
    {
        private static AutoResetEvent waitHandler = new AutoResetEvent(false);
        private static readonly object _locker = new object();
        public bool IsTaskCompleted { get; private set; }
        public bool IsPending { get; private set; } = true;
        public bool IsAborted { get; set; } = false;

        private List<Func<bool>> _functionsQueue;

        private Thread _thread;


        public QueueWorker(ref List<Func<bool>> functionsQueue, string name)
        {
            _functionsQueue = functionsQueue;
            _thread = new Thread(ProcessThread);
            _thread.Name = name;
            _thread.Start();
        }

        public QueueWorker()
        {
            _thread = new Thread(ProcessThread);
            _thread.Name = "DUDU";
            _thread.Start();
        }

        private void ProcessThread()
        {
            Console.WriteLine("Started");
            while (!IsAborted)
            {
                CheckQueue();
            }
        }

        public bool Execute(Func<bool> action)
        {
            bool status = false;
            try
            {
                Console.WriteLine($"{_thread.Name} have taken task");
                IsPending = false;
                if (action.Invoke())
                {
                    IsTaskCompleted = true;
                    Console.WriteLine($"{_thread.Name} completed a task");
                    status = true;
                }
                IsPending = true;
                IsTaskCompleted = false;
            }
            catch(Exception ex)
            {
                status = false;
            }
            return status;
        }

        public void AbortThread()
        {
            IsAborted = true;
            _thread.Abort();
            Console.WriteLine($"{_thread.Name} is released.");
        }

        private void CheckQueue()
        {
            if (IsPending)
            {
                Func<bool> func = null;
                lock (_locker)
                {
                    if (_functionsQueue.Count > 0)
                    {
                        Console.WriteLine($"Tasks left: {_functionsQueue.Count}");
                        func = _functionsQueue.FirstOrDefault();
                        if (func != null)
                        {
                            _functionsQueue.Remove(func);
                        }
                    }
                }
                if (func != null && !Execute(func))
                {
                    lock (_locker)
                    {
                        _functionsQueue.Add(func);
                    }
                }
            }
        }
        
    }
}
