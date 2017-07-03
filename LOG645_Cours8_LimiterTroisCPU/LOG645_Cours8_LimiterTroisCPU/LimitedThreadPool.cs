using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOG645_Cours8_LimiterTroisCPU
{
    public class LimitedThreadPool
    {
        private readonly SemaphoreSlim _semaphore;
        
        public LimitedThreadPool(int maxConcurrency)
        {
            _semaphore = new SemaphoreSlim(maxConcurrency);
        }

        public Thread Run(Action action)
        {
            Thread thread = new Thread(() =>
            {
                _semaphore.Wait();

                try
                {
                    action();
                }
                catch (Exception)
                {
                    Console.WriteLine("Boom!");    
                }
                finally
                {
                    _semaphore.Release();
                }
            });

            thread.Start();

            return thread;
        }
    }
}
