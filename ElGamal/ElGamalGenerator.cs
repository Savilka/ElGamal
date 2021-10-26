using System;
using System.Collections.Generic;
using static ElGamalGenerator.Utils;

namespace ElGamalGenerator
{
    public class ElGamalGenerator
    {
        private static readonly Random Random = new();

        internal int PrivateKey;

        public ElGamalGenerator()
        {
            PublicKeys["y"] = 0;
            PublicKeys["g"] = 0;
            PublicKeys["p"] = 0;
            PrivateKey = 0;
        }

        internal Dictionary<string, int> PublicKeys { get; } = new();

        public void Run()
        {
            
            Console.WriteLine("Choose option:");
            Console.WriteLine("1: Generate prime number");
            Console.WriteLine("2: Enter from the console");
            var option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    var rndSeed = Random.Next(8, 31);
                    
                    PublicKeys["p"] = GeneratePrime(rndSeed);
                    PublicKeys["g"] = GeneratePrimitiveRoot(PublicKeys["p"]);
                    PrivateKey = GeneratePrivateKey(PublicKeys["p"], Random);
                    PublicKeys["y"] = GenerateYKey(PublicKeys["p"], PublicKeys["g"], PrivateKey);
                    
                    break;
                case 2:
                    Console.Write("Enter the number 'p': ");
                    var pCandidate = Convert.ToInt32(Console.ReadLine());
                    while (!IsMillerRabinPass(pCandidate, (int) Math.Log2(pCandidate) + 1, Random))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("p is not prime number. Enter the prime number");
                        Console.ResetColor();
                        pCandidate = Convert.ToInt32(Console.ReadLine());
                    }
                    PublicKeys["p"] = pCandidate;
                    
                    Console.Write("Enter the number 'g': ");
                    var gCandidate = Convert.ToInt32(Console.ReadLine());
                    while (!CheckIsPrimitiveRoot(pCandidate, gCandidate))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("g is not primitive root. Enter the primitive root");
                        Console.ResetColor();
                        gCandidate = Convert.ToInt32(Console.ReadLine());
                    }

                    PublicKeys["g"] = gCandidate;
                    PrivateKey = GeneratePrivateKey(pCandidate, Random);
                    PublicKeys["y"] = GenerateYKey(PublicKeys["p"], PublicKeys["g"], PrivateKey);
                    break;
            }
            
        }

        private static int GenerateYKey(int p, int g, int x)
        {
            return ModularPow(g, x, p);
        }

        private static int GeneratePrivateKey(int p, Random r)
        {
            var x = r.Next(2, p - 1);
            return x;
        }

        private static bool IsMillerRabinPass(int candidate, int trialsNum, Random r)
        {
            if (candidate % 2 == 0)
            {
                return false;
            }

            var maxDivisionByTwo = 0;
            var evenComponent = candidate - 1;

            while (evenComponent % 2 == 0)
            {
                evenComponent /= 2;
                maxDivisionByTwo++;
            }

            for (var i = 0; i < trialsNum; i++)
            {
                var roundTester = r.Next(2, candidate - 1);

                if (TrialComposite(roundTester, candidate, maxDivisionByTwo, evenComponent))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool TrialComposite(int roundTester, int candidate, int maxDivisionsByTwo, int evenComponent)
        {
            var x = ModularPow(roundTester, evenComponent, candidate);

            if (x == 1 || x == candidate - 1)
            {
                return false;
            }

            for (var i = 0; i < maxDivisionsByTwo - 1; i++)
            {
                x = ModularPow(x, 2, candidate);

                if (x == 1)
                {
                    return true;
                }

                if (x == candidate - 1)
                {
                    return false;
                }
            }

            return true;
        }

        private static int GeneratePrime(int seed)
        {
            var firstPrimes = ProceedSieveOfEratosthenes(350);

            while (true)
            {
                var primeCandidate = GetLowLevelPrime(seed, firstPrimes);

                if (!IsMillerRabinPass(primeCandidate, (int) Math.Log2(primeCandidate) + 1, Random))
                {
                    continue;
                }

                return primeCandidate;
            }
        }

        private static int GetLowLevelPrime(int n, int[] firstPrimes)
        {
            while (true)
            {
                var primeCandidate = GenerateNBitNum(n, Random);
                var lowLevelCheck = true;

                foreach (var divisor in firstPrimes)
                {
                    if (primeCandidate % divisor == 0 && divisor * divisor <= primeCandidate)
                    {
                        lowLevelCheck = false;
                        break;
                    }
                }

                if (lowLevelCheck)
                {
                    return primeCandidate;
                }
            }
        }

        private static int GenerateNBitNum(int n, Random r)
        {
            return r.Next(FastPow(2, n - 1) + 1, FastPow(2, n) - 1);
        }

        private static int GeneratePrimitiveRoot(int primeNumber)
        {
            
            var phi = primeNumber - 1;
            var factors = Factorize(phi);
            
            for (var res = 2; res <= primeNumber; res++)
            {
                var isPrimitive = true;

                for (var i = 0; i < factors.Count && isPrimitive; i++)
                {
                    isPrimitive &= ModularPow(res, phi / factors[i], primeNumber) != 1;
                }

                if (isPrimitive)
                {
                    return res;
                }
            }

            return -1;
        }

        private static bool CheckIsPrimitiveRoot(int primeNumber, int candidateRoot)
        {
            
            if (Gcd(candidateRoot, primeNumber) != 1)
            {
                return false;
            }
            
            var phi = primeNumber - 1;

            if (ModularPow(candidateRoot, phi / 2, primeNumber) == 1)
            {
                return false;
            }
            
            var factors = Factorize(phi);

            foreach (var factor in factors)
            {
                if (ModularPow(candidateRoot, phi / factor, primeNumber) == 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}