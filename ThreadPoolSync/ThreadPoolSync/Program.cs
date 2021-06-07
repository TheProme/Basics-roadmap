using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolSync
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool threadPool = new ThreadPool(5);
            for (int i = 0; i < 50; i++)
            {
                threadPool.AddJob(Job);
            }
            Console.ReadLine();
        }

        static Random rnd = new Random();
        public static void Job()
        {
            var rndVal = rnd.Next(100, 2000);
            Thread.Sleep(rndVal);
            Console.WriteLine($"Job is done. Time {rndVal}");
        }
    }
}
