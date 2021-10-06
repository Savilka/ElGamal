using System;

namespace ElGamalGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new ElGamalGenerator();
            generator.Run();
            Console.WriteLine(generator.PublicKeys["p"]);
            Console.WriteLine(generator.PublicKeys["g"]);
            Console.WriteLine(generator.PublicKeys["y"]);
            Console.WriteLine(generator.PrivateKey);
        }
    }
}