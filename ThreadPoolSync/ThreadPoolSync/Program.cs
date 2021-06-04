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
            ThreadPool threadPool = new ThreadPool(10);
            threadPool.AddActions(Job, 20);
            Console.ReadLine();
        }

        public static bool Job()
        {
            for (int i = 0; i < 10000000; i++)
            {
                string str = i.ToString();
            }
            Console.WriteLine("Job is done");
            return true;
        }
    }
}
