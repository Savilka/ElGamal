using System;
using System.Collections;
using System.Collections.Generic;

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

        //  Add BigInteger!
        public static int ModularPow(int baseNum, int power, int modulus)
        {
            var res = 1;
            for (int i = 0; i < power; i++)
            {
                res = (res * baseNum) % modulus;
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns> prime numbers less or equal to n </returns>
        public static int[] ProceedSieveOfEratosthenes(int n)
        {
            var numOfPrimes = (int) (n / (Math.Log(n) - 1));
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

            for (int i = 2; i <= n; i++)
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