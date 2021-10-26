using System;
using System.Collections.Generic;
using System.Linq;
using static ElGamalGenerator.Utils;

namespace ElGamalGenerator
{
    public class ElGamalEncryptor
    {
        private static readonly Random Random = new();
        
        public ElGamalEncryptor(int p, int g, int y)
        {
            PublicKeys["y"] = y;
            PublicKeys["g"] = g;
            PublicKeys["p"] = p;
            
            CipherText["a"] = 0;
            CipherText["b"] = 0;
        }

        internal Dictionary<string, int> CipherText { get; } = new();
        
        private Dictionary<string, int> PublicKeys { get; } = new();
        
        public void Run()
        {
            var k = GenerateSessionKey(Random);
            Console.Write("Enter the message 'M': ");
            var m = Convert.ToInt32(Console.ReadLine());
            while (m > PublicKeys["p"])
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("m is so long :(");
                Console.ResetColor();
                m = Convert.ToInt32(Console.ReadLine());
            }
            Encrypt(m, k);
            Console.WriteLine(CipherText["a"]);
            Console.WriteLine(CipherText["b"]);
        }

        private int GenerateSessionKey(Random r)
        {
            var coprime = false;
            var pFactors = Factorize(PublicKeys["p"] - 1);
            var k = 0;
            while (!coprime)
            {
                k = r.Next(2, PublicKeys["p"] - 1);
                var kFactors = Factorize(k);
                var coprimeCounter = pFactors.Count(factor => !kFactors.Contains(factor));

                if (coprimeCounter == pFactors.Count)
                {
                    coprime = true;
                }
            }
            
            Console.WriteLine("P:" + PublicKeys["p"]);
            return k;
        }

        private void Encrypt(int m, int k)
        {
            CipherText["a"] = ModularPow(PublicKeys["g"], k, PublicKeys["p"]);
            CipherText["b"] = ModularPow(PublicKeys["y"], k, PublicKeys["p"], m);
        }
    }
}