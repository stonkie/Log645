using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOG645_Cours8_LimiterTroisCPU
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Thread> threads = new List<Thread>();

            LimitedThreadPool pool = new LimitedThreadPool(3);
            
            for (int actionIndex = 0; actionIndex < 7; actionIndex++)
            {
                int index = actionIndex;
                threads.Add(pool.Run(() =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Thread {index} finished at {DateTime.Now.ToString("o")} .");
                }));
            }

            threads.ForEach(thread => thread.Join());

            Console.WriteLine("Finished all!");
            Console.ReadKey();
        }
    }
}
