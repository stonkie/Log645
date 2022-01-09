using System;
using System.Diagnostics;

namespace LOG645_Cours5_OptimisationAccèsMémoire
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var maxX = 10000;
            var maxY = 10000;

            var matrix = new int[maxX, maxY];

            var watch = new Stopwatch();
            watch.Start();

            for (var x = 0; x < maxX; x++)
            for (var y = 0; y < maxY; y++)
                matrix[x, y] = 42;

            watch.Stop();
            Console.WriteLine($"Time : {watch.Elapsed}");
            Console.ReadLine();
        }
    }
}