using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPoolSync
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool threadPool = new ThreadPool();
            for (int i = 0; i < 10; i++)
            {
                threadPool.AddWorker();
            }
            Console.ReadLine();
            for (int i = 0; i < 10; i++)
            {
                threadPool.AddJob(Job);
            }
            Console.ReadLine();
        }

        public static void Job()
        {
            for (int i = 0; i < 10000000; i++)
            {
                string str = i.ToString();
            }
        }
    }
}
