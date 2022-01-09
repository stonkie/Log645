using System;
using System.Linq;

namespace LOG645_Cours5_DestructionDépendances
{
    internal class Program
    {
        private static readonly Random random = new Random();

        private static void Main(string[] args)
        {
            var count = 10000;
            int[] sum = new int[count];
						double[] x = new double[count];

            for (var i = 0; i < count; i++)
            {
                x[i] = random.NextDouble();
                var y = random.NextDouble();

                if (Math.Sqrt(x[i] * x[i] + y * y) <= 1.0)
                    sum[i] = 1;
                else
                {
                    sum[i] = 0;
                }
            }

            int totalSum = sum.Sum();

            Console.WriteLine($"Pi estimate : {4.0 * totalSum / count}");
            Console.ReadLine();
        }
    }
}