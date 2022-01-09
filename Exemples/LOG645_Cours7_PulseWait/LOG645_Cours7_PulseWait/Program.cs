using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LOG645_Cours7_PulseWait
{
    internal class Program
    {
        private class SomeJob
        {
            public int Id { get; set; }
        }

        private static readonly Queue<SomeJob> Jobs = new Queue<SomeJob>();
        private static readonly Object JobsLock = new Object();

        private static void Main(string[] args)
        {
            StartConsumer();
            StartConsumer();
            StartConsumer();
            StartConsumer();
            StartConsumer();

            Thread.Sleep(100);

            StartProducer();
        }

        private static void StartProducer()
        {
            var producerThread = new Thread(() =>
            {
                for (int jobId = 0; jobId < 100; jobId++)
                {
                    lock (JobsLock)
                    {
                        Console.WriteLine($"     Producing job {jobId}");
                        Jobs.Enqueue(new SomeJob() {Id = jobId});
                        Monitor.Pulse(JobsLock);
                    }
                    Thread.Sleep(10);
                }
            });

            producerThread.Start();
        }

        private static void StartConsumer()
        {
            var consumerThread = new Thread(() =>
            {
                while (true)
                {
                    SomeJob job;

                    lock (JobsLock)
                    {
                        while (Jobs.Count == 0)
                        {
                            Monitor.Wait(JobsLock);
                        }

                        job = Jobs.Dequeue();
                    }

                    Console.WriteLine($"Consuming job {job.Id}");
                    Thread.Sleep(100);
                }
            });

            consumerThread.Start();
        }
    }
}