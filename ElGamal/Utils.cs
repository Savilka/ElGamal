using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ElGamalGenerator
{
    public static class Utils
    {
        public static int FastPow(int baseNum, int power)
        {
            var res = 1;

            while (power > 0)
            {
                if (power % 2 == 0)
                {
                    power /= 2;
                    baseNum *= baseNum;
                }
                else
                {
                    power -= 1;
                    res *= baseNum;
                    power /= 2;
                    baseNum *= baseNum;
                }
            }

            return res;
        }

        public static int ModularPow(int baseNum, int power, int modulus, int multiplier = 1)
        {
            BigInteger bigBaseNum = baseNum;
            BigInteger bigModulus = modulus;
            BigInteger bigMultiplier = multiplier;
            BigInteger res = 1;

            while (power > 0)
            {
                if (power % 2 == 0)
                {
                    power /= 2;
                    bigBaseNum = bigBaseNum * bigBaseNum % bigModulus;
                }
                else
                {
                    power -= 1;
                    res = bigBaseNum * res % bigModulus;
                    power /= 2;
                    bigBaseNum = bigBaseNum * bigBaseNum % bigModulus;
                }
            }

            res = res * bigMultiplier % bigModulus;
            return (int) res;
        }

        public static List<int> Factorize(int factorizeNumber)
        {
            var factors = new List<int>();
            var sqrt = (int) Math.Sqrt(factorizeNumber) + 1;
            factors.Add(factorizeNumber);
            for (var i = 2; i  <= sqrt; i++)
            {
                if (factorizeNumber % i != 0) continue;
                factors.Add(i);

                while (factorizeNumber % i == 0)
                {
                    factorizeNumber /= i;
                }
            }

            return factors;
        }

        public static int Gcd(int a, int b)
        {
            return b != 0 ? Gcd(b, a % b) : a;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns> prime numbers less or equal to n </returns>
        public static int[] ProceedSieveOfEratosthenes(int n)
        {
            var primes = new List<int>();
            var isPrime = new BitArray(n + 1, true);

            for (var p = 2; p * p <= n; p++)
            {
                if (isPrime[p])
                {
                    for (var i = p * p; i <= n; i += p)
                    {
                        isPrime[i] = false;
                    }
                }
            }

            for (var i = 2; i <= n; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                }
            }

            return primes.ToArray();
        }
    }
}