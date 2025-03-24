# Composite-Characters-in-Nepali(C#)
Composite characters (संयुक्त अक्षर) in a word consist of two or more consonants combined with a vowel. They are essential when working with various tools like font conversion (TTF to Unicode and vice versa), spell checking, and text justification. These characters play a crucial role in maintaining character semantics during such processes.
- For example: a Word "राष्ट्रियताले" consists of "रा ष्ट्रि य ता ले" composite characters.
  
This work uses state-machine based approach to solve this problem and implemented in C# for MSWord plugin.
##Sample Usage:
```cs
﻿using Syllabifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
```

