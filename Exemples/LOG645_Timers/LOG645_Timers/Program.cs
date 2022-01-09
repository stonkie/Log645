using System;
using System.Security.Cryptography;
using System.Threading;

namespace LOG645_Timers
{
    internal class Program
    {
        private static int runningCount = 0;

        private static void Main(string[] args)
        {
            Timer timer = new Timer(TimerCallback);
            timer.Change(0, 1000);
            Thread.Sleep(5000);
            timer.Dispose();
            Thread.Sleep(1000);
            Console.WriteLine($"Running Count : {runningCount}");
        }

        private static void TimerCallback(object state)
        {
            runningCount++;
            Console.WriteLine("Entering Timer");

            DoStuff();
            
            Console.WriteLine("Exiting Timer");
            runningCount--;
        }

        private static void DoStuff()
        {
            uint randomValue;

            using (RNGCryptoServiceProvider random = new RNGCryptoServiceProvider()) 
            { 
                byte[] bytes = new byte[5];    
                random.GetBytes(bytes);
                randomValue = BitConverter.ToUInt32(bytes, 0);
            }

            Thread.Sleep((int) (randomValue % 2000));
        }
    }
}