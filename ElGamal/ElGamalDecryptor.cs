using System;
using System.Collections.Generic;
using static ElGamalGenerator.Utils;

namespace ElGamalGenerator
{
    public class ElGamalDecryptor
    {
        public ElGamalDecryptor(Dictionary<string, int> cipherText, int p, int x)
        {
            CipherText["a"] = cipherText["a"];
            CipherText["b"] = cipherText["b"];
            _p = p;
            _privateKey = x;
        }

        private readonly int _p;
        private readonly int _privateKey;
        private Dictionary<string, int> CipherText { get; } = new();

        public void Run()
        {
            var m = Decrypt();
            Console.WriteLine(m);
        }

        private int Decrypt()
        {
           return ModularPow(CipherText["a"], _p - 1 - _privateKey, _p, CipherText["b"]);
        }
    }
}