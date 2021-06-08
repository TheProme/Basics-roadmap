using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolSync
{
    internal class QueueWorker
    {
        private EventWaitHandle _jobWaitHandler = new AutoResetEvent(false);

        public delegate void WorkStatus(QueueWorker worker);
        public event WorkStatus NotifyCompletion;

        private Thread _thread;
        public Action PickedAction;

        public QueueWorker(string name)
        {
            Name = name;
            _thread = new Thread(new ThreadStart(CompleteJob)) { Name = this.Name };
            _thread.Start();
        }

        public string Name { get; private set; }

        public bool CompletedTask { get; private set; } = false;

        private void CompleteJob()
        {
            while (_thread.IsAlive)
            {
                if (PickedAction != null)
                {
                    try
                    {
                        PickedAction.Invoke();
                        Console.WriteLine($"{_thread.Name} completed a task");
                        PickedAction = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{_thread.Name} failed to complete a task");
                    }
                    finally
                    {
                        CompletedTask = true;

                        NotifyCompletion.Invoke(this);
                        CompletedTask = false;

                        _jobWaitHandler.WaitOne();
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            Console.WriteLine($"{_thread.Name} FINISHED");
        }

        public void Execute(Action job)
        {
            _jobWaitHandler.Set();
            PickedAction = job;
        }
    }
}
