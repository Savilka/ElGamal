using System;

namespace ElGamalGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new ElGamalGenerator();
            generator.Run();
            var encryptor = new ElGamalEncryptor(generator.PublicKeys["p"],generator.PublicKeys["g"],generator.PublicKeys["y"]);
            encryptor.Run();
            var decryptor = new ElGamalDecryptor(encryptor.CipherText, generator.PublicKeys["p"], generator.PrivateKey);
            decryptor.Run();


        }
    }
}