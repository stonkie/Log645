using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log645_Cours5_Torsion
{
    class Program
    {
        static void Main(string[] args)
        {
            int maxX = 10;
            int maxY = 10;
            
            PrintVanilla(maxX, maxY);

            Console.WriteLine();

            PrintSkewed(maxX, maxY);

            Console.ReadLine();
        }

        private static void PrintSkewed(int maxX, int maxY)
        {
            var matrix = Enumerable.Range(0, maxX).Select(index => Enumerable.Repeat(0, maxY).ToArray()).ToArray();

            for (int y = 0; y < maxY + maxX - 1; y++)
            {
                for (int x = Math.Max(0, y - maxY + 1); x <= Math.Min(y, maxX - 1); x++)
                {
                    if (x == 0 || (y - x) == 0)
                    {
                        matrix[x][y - x] = 5;
                    }
                    else
                    {
                        matrix[x][y - x] = matrix[x - 1][y - x] + matrix[x][y - x - 1];
                    }
                }
            }

            for (int x = 0; x < maxX; x++)
            {
                Console.WriteLine(String.Join(", ", matrix[x]));
            }
        }

        private static void PrintVanilla(int maxX, int maxY)
        {
            var matrix = Enumerable.Range(0, maxX).Select(index => Enumerable.Repeat(0, maxY).ToArray()).ToArray();

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    if (x == 0 || y == 0)
                    {
                        matrix[x][y] = 5;
                    }
                    else
                    {
                        matrix[x][y] = matrix[x - 1][y] + matrix[x][y - 1];
                    }
                }
            }

            for (int x = 0; x < maxX; x++)
            {
                Console.WriteLine(String.Join(", ", matrix[x]));
            }
        }
    }
}
