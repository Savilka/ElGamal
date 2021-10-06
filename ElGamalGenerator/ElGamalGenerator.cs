using System;
using System.Collections.Generic;
using System.Numerics;
using static ElGamalGenerator.Utils;

namespace ElGamalGenerator
{
    public class ElGamalGenerator
    {
        private static readonly Random Random = new();

        private int _p, _g;

        // How do we provide private key? As long as we don't have friend in C#?
        // Can we be inherited by ElGamalEncryptionSystem and make this protected?
        public int PrivateKey;

        public ElGamalGenerator()
        {
            PublicKeys["y"] = 0;
            PublicKeys["g"] = 0;
            PublicKeys["p"] = 0;
            PrivateKey = 0;
        }

        public Dictionary<string, int> PublicKeys { get; } = new();

        public void Run()
        {
            var rndSeed = Random.Next(8, 31);
            var p = GeneratePrime(rndSeed);
            PublicKeys["p"] = p;
            // TODO: Add more content
            // pass
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
            var x = (int) BigInteger.ModPow(roundTester, evenComponent, candidate);

            if (x == 1 || x == candidate - 1)
            {
                return false;
            }

            for (int i = 0; i < maxDivisionsByTwo - 1; i++)
            {
                x = (int) BigInteger.ModPow(x, 2, candidate);

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
            // if we pre-generate first primes we can run faster

            while (true)
            {
                var primeCandidate = GenerateNBitNum(n, Random);
                bool lowLevelCheck = true;
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
            return r.Next(FastPow(2, n - 1) + 1, FastPow(2, n));
        }

        // TODO: Moderate access to the function
        // Mb protected will be good decision?
        public void Share()
        {
            // pass
        }
    }
}