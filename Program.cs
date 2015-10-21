using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kikkare
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new WebParser();
            Console.Write("Parsing web sites for header data...");

            parser.Load(@"http://www.iltalehti.fi/etusivu/", Encoding.Default);
            IEnumerable<string> ilHeaders = parser.GetHeaders();
            parser.Load(@"http://www.iltasanomat.fi/", Encoding.UTF8);
            IEnumerable<string> isHeaders = parser.GetHeaders("h2");

            int count = ilHeaders.Count() + isHeaders.Count();
            Console.WriteLine("Done! Got " + count + " headers.");
            Console.Write("Generating Markov model...");
            var gen = new TextGenerator();
            gen.ReadInput(ilHeaders);
            gen.ReadInput(isHeaders);
            Console.WriteLine("Done!");
            Console.WriteLine("Press any key to generate sentences (press esc to quit):\n");


            int avgLength = (ilHeaders.GetAverageLength() + isHeaders.GetAverageLength()) / 2;
            while (Console.ReadKey().Key != ConsoleKey.Escape)
                Console.WriteLine(gen.GenerateSentence(avgLength));

            //for (int i = 0; i < 100; i++)
            //{
            //    string sentence = gen.GenerateSentence(avgLength);
            //    Console.WriteLine(sentence);
            //}
        }
    }
}
