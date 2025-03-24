using Syllabifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyllabifierTest
{
    class Program
    {


        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.Unicode;

            String wordText = "राष्ट्रियताको";
            List<String> boundaries = SyllableTokenizer.findAllBoundaries(wordText);

            foreach(String w in boundaries)
            {
                Console.WriteLine(w);
            }

            Console.ReadKey();
        }
    }
}
