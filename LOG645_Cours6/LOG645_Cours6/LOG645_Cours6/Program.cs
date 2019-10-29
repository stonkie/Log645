using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LOG645_Cours6
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int[,] matrice = new int[8, 12];

            for (int y = 0; y < 8; y++)
            {
                matrice[y, 0] = 4;
            }
            
            // Partie à paralléliser
            for (int y = 0; y < 8; y++)
            {
                for (int x = 1; x < 12; x++)
                {
                    matrice[y, x] = calcul(matrice[y, x - 1]);
                }   
            }
            
            
        }

private static void Mutex_AbandonnedExample()
{
    Mutex mutex = new Mutex();
    List<Thread> threads = new List<Thread>();

    for (int i = 0; i < 10; i++)
    {
        threads.Add(new Thread(() => { mutex.WaitOne(); }));
        threads[i].Start();
    }

    threads.ForEach(thread => thread.Join());
}

private static void Semaphore_AbandonnedExample()
{
    Semaphore semaphore = new Semaphore(0, 1);
    List<Thread> threads = new List<Thread>();

    for (int i = 0; i < 10; i++)
    {
        threads.Add(new Thread(() => { semaphore.WaitOne(); }));
        threads[i].Start();
    }

    threads.ForEach(thread => thread.Join());
}
    }
}