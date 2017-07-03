using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOG645_Cours8_ReadWriteLock
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryCache cache = new MemoryCache(1024, 8);

            Thread writerThread = new Thread(() =>
            {
                Enumerable.Range(1, 100)
                    .AsParallel()
                    .ForAll(value =>
                    {
                        using (Stream stream = cache.OpenStream())
                        {
                            stream.Write(Enumerable.Repeat((byte) value, 100).ToArray(), 50, 100);
                        }

                        Console.WriteLine($"Writing {value}");
                    });
            });

            Thread readerThread = new Thread(() =>
            {
                for (int testIndex = 0; testIndex < 100; testIndex++)
                {
                    byte[] readData = new byte[100];
                    using (Stream stream = cache.OpenStream())
                    {
                        stream.Read(readData, 50, 100);
                    }

                    Console.WriteLine($"Was {readData[0]} atomic = {readData.All(value => value == readData[0])}");
                }
            });

            writerThread.Start();
            readerThread.Start();
            
            writerThread.Join();
            readerThread.Join();

            Console.ReadLine();
        }
    }
}
