using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolSync
{
    public class QueueWorker
    {
        public delegate void WorkStatus(QueueWorker worker);
        public event WorkStatus NotifyCompletion;
        private Thread _thread;
        public QueueWorker(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public bool HasFinishedTask { get; private set; } = false;

        private void CompleteJob(Action job)
        {
            try
            {
                job.Invoke();
                Console.WriteLine($"{_thread.Name} completed a task");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{_thread.Name} failed to complete a task");
            }
            finally
            {
                HasFinishedTask = true;
                NotifyCompletion.Invoke(this);
                HasFinishedTask = false;
            }
        }

        public void Execute(Action job)
        {
            try
            {
                _thread = new Thread(new ThreadStart(()=> { CompleteJob(job); })) { Name = this.Name };
                _thread.Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{_thread.Name} failed to start");
            }
        }
    }
}
