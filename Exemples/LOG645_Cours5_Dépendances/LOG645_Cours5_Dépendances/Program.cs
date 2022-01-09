using System;

namespace LOG645_Cours5_Dépendances
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            uint[] key = {67, 93, 8372920, 272364891};

            var value = ulong.Parse(Console.ReadLine());

            var encrypted = Encrypt(value, key);
            Console.WriteLine($"Encrypted = {encrypted}");
            var decrypted = Decrypt(encrypted, key);
            Console.WriteLine($"Decrypted = {decrypted}");

            Console.ReadLine();
        }

        private static ulong Encrypt(ulong value, uint[] k)
        {
            var v0 = (uint) (value & uint.MaxValue);
            var v1 = (uint) ((value >> 32) & uint.MaxValue);
            uint sum = 0;

            var delta = 0x9e3779b9; // a key schedule constant
            var k0 = k[0]; // cache key
            var k1 = k[1];
            var k2 = k[2];
            var k3 = k[3];
            
            for (var i = 0; i < 32; i++)
            {
                sum += delta;
                var v0_0 = ((v1 << 4) + k0);
                var v0_1 = (v1 + sum);
                var v0_2 = ((v1 >> 5) + k1);

                v0 += v0_0 ^ v0_1 ^ v0_2;

                var v1_0 = ((v0 << 4) + k2);
                var v1_1 = (v0 + sum);
                var v1_2 = ((v0 >> 5) + k3);

                v1 += v1_0 ^ v1_1 ^ v1_2;
            }

            return v0 + ((ulong) v1 << 32);
        }

        private static ulong Decrypt(ulong value, uint[] key)
        {
            var v0 = (uint) (value & uint.MaxValue);
            var v1 = (uint) ((value >> 32) & uint.MaxValue);

            var sum = 0xC6EF3720; /* set up */
            var delta = 0x9e3779b9; /* a key schedule constant */
            var k0 = key[0];
            var k1 = key[1];
            var k2 = key[2];
            var k3 = key[3];

            for (var i = 0; i < 32; i++)
            {
                v1 -= ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
                v0 -= ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
                sum -= delta;
            }

            return v0 + ((ulong) v1 << 32);
        }
    }
}