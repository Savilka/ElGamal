using static System.Math;

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
            // According to 'Prime Number Theorem' approximation of number of primes <= n
            // Thanks https://math.stackexchange.com/questions/264544/how-to-find-number-of-prime-numbers-up-to-to-n
            var numOfPrimes = (int) (n / Log(n - 1));
            var primes = new int[numOfPrimes];
            var isPrime = new bool[n + 1];

            for (var i = 0; i < n; i++)
            {
                isPrime[i] = true;
            }

            var primeIdx = 0;
            for (var p = 2; p * p <= n; p++)
            {
                if (isPrime[p])
                {
                    primes[primeIdx] = p;

                    for (var i = p * p; i <= n; i += p)
                    {
                        isPrime[i] = false;
                    }
                }

                primeIdx++;
            }

            return primes;
        }
    }
}