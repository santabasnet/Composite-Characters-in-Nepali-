using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syllabifier
{
    public class TokenizerData
    {
        /**
        * Data structure to check for different character (substring) during
        * character grouping
        */
        private readonly HashSet<String> WHOLE_CONSONANTS;
        private readonly HashSet<String> WHOLE_VOWELS;
        private readonly HashSet<String> DEPENDENT_VOWELS;
        private readonly HashSet<String> DEVANAGARI_NUMBERS;
        private readonly HashSet<String> JOINING_SYMBOLS;
        private readonly HashSet<String> SPECIAL_SYMBOLS;
        private readonly HashSet<String> PUNCTUATION_MARKS;
        private readonly String HALANTA_SYMBOL;
        private readonly String NON_JOINING_CHAR;
        private readonly String JOINING_CHAR;

        /// <summary>
        /// Default Constructor, loads all the resource data from the given file.
        /// </summary>
        public TokenizerData()
        {
            this.WHOLE_CONSONANTS = new HashSet<String>(UnicodeTokenizerProperties.GetWholeConsonants());
            this.WHOLE_VOWELS = new HashSet<String>(UnicodeTokenizerProperties.GetWholeVowels());
            this.DEPENDENT_VOWELS = new HashSet<String>(UnicodeTokenizerProperties.GetDependentVowels());
            this.DEVANAGARI_NUMBERS = new HashSet<String>(UnicodeTokenizerProperties.GetDevanagariNumbers());
            this.JOINING_SYMBOLS = new HashSet<String>(UnicodeTokenizerProperties.GetJoiningSymbols());
            this.SPECIAL_SYMBOLS = new HashSet<String>(UnicodeTokenizerProperties.GetSpecialSymbols());
            this.PUNCTUATION_MARKS = new HashSet<String>(
            //Arrays.asList(",", "'", "\"", "!", "—", "।।")
            //Arrays.asList(",", "'", "\"", "!", "-", "—", "?", "(", ")", "]", "[", "{", "}", ".", ";", "।", "।।")
            );
            this.HALANTA_SYMBOL = UnicodeTokenizerProperties.GetHalanta();
            this.NON_JOINING_CHAR = UnicodeTokenizerProperties.GetNonJoiningCharacter();
            this.JOINING_CHAR = UnicodeTokenizerProperties.GetJoiningCharacter();
        }

        public Boolean IsWholeConsonant(String chStr)
        {
            return WHOLE_CONSONANTS.Contains(chStr);
        }

        public Boolean IsWholeVowel(String chStr)
        {
            return WHOLE_VOWELS.Contains(chStr);
        }

        public Boolean IsDependentVowel(String chStr)
        {
            return DEPENDENT_VOWELS.Contains(chStr);
        }

        public Boolean IsDevanagariNumber(String chStr)
        {
            return DEVANAGARI_NUMBERS.Contains(chStr);
        }

        public Boolean IsJoinableSymbol(String chStr)
        {
            return JOINING_SYMBOLS.Contains(chStr);
        }

        public Boolean IsSpecialSymbol(String chStr)
        {
            return SPECIAL_SYMBOLS.Contains(chStr);
        }

        public Boolean IsHalanta(String chStr)
        {
            return HALANTA_SYMBOL.Equals(chStr);
        }

        public Boolean IsZWNJ(String chStr)
        {
            return NON_JOINING_CHAR.Equals(chStr);
        }

        public Boolean IsZWJ(String chStr)
        {
            return JOINING_CHAR.Equals(chStr);
        }

        public Boolean IsPunctuationMark(String chStr)
        {
            return PUNCTUATION_MARKS.Contains(chStr);
        }

    }

    /// <summary>
    /// Tokenizer Properties, stores the unicode characters and their categories.
    /// 
    /// #Devanagari Character Classification
    /// #This classification allows us to group joining/half characters in a single place
    /// #with a whole consonant (independent character) in a written form.
    /// 
    /// # Copyright: 2015 Integrated ICT Pvt. Ltd.
    /// # Kathmandu, Nepal
    /// # http://www.integratedict.com.np
    /// #Author : Santa Basnet
    /// </summary>
    public class UnicodeTokenizerProperties
    {
        /// <summary>
        /// Consonant
        /// </summary>
        private static readonly List<Char> DevanagariConsonants = new List<Char> {
            '\u0915', '\u0916', '\u0917', '\u0918', '\u0919', '\u091a', '\u091b', '\u091c', '\u091d', '\u091e', '\u091f', '\u0920',
            '\u0921', '\u0922', '\u0923', '\u0924', '\u0925', '\u0926', '\u0927', '\u0928', '\u0929', '\u092a', '\u092b', '\u092c',
            '\u092d', '\u092e', '\u092f', '\u0930', '\u0931', '\u0932', '\u0933', '\u0934', '\u0935', '\u0936', '\u0937', '\u0938',
            '\u0939', '\u0958', '\u0959', '\u095a', '\u095b', '\u095c', '\u095d', '\u095e', '\u095f'
        };

        /// <summary>
        /// Independent Vowels
        /// </summary>
        private static readonly List<Char> DevanagariVowels = new List<Char> {
            '\u0905', '\u0906', '\u0907', '\u0908', '\u0909', '\u090a', '\u090b', '\u0960', '\u090c', '\u0961', '\u090f', '\u0910',
            '\u0913', '\u0914', '\u090d', '\u090e', '\u0911', '\u0912'
        };

        /// <summary>
        /// #Dependent Vowels
        /// </summary>
        private static readonly List<Char> DevanagariDependentVowels = new List<Char> {
            '\u093e', '\u093f', '\u0940', '\u0941', '\u0942', '\u0943', '\u0944', '\u0945', '\u0946', '\u0947', '\u0948', '\u0949', '\u094a', '\u094b', '\u094c', '\u0962', '\u0963'
        };


        /// <summary>
        /// Numbers
        /// </summary>
        private static readonly List<Char> DevanagariNumbers = new List<Char> {
            '\u0966', '\u0967', '\u0968', '\u0969', '\u096a', '\u096b', '\u096c', '\u096d', '\u096e', '\u096f'
        };

        /// <summary>
        /// Special Joining Symbols
        /// </summary>
        private static readonly List<Char> SpecialJoiningSymbols = new List<Char> {
            '\u0901', '\u0902', '\u0903', '\u093c', '\u093d', '\u0970', '\u0971', '\u0953', '\u0954', '\u0951'
        };

        /// <summary>
        /// Special Symbols
        /// </summary>
        private static readonly List<Char> SpecialSymbols = new List<Char> { '\u0950', '\u25cc', '\u2219', '\u2212', '\u0964', '\u0952', '\u0965' };

        /// <summary>
        /// Halanta
        /// </summary>
        private static readonly List<Char> Halanta = new List<Char> { '\u094d' };

        /// <summary>
        /// Zero with Non Joining Character
        /// </summary>
        private static readonly List<Char> NonJoinChar = new List<Char> { '\u200C' };

        /// <summary>
        /// Zero with Joiner Character
        /// </summary>
        private static readonly List<Char> JoinChar = new List<Char> { '\u200D' };

        /// <summary>
        /// Returns all the consonants of the devanagari text.
        /// </summary>
        /// <returns></returns>
        public static List<String> GetWholeConsonants()
        {          
            return CharToStrList(DevanagariConsonants);
        } 

        public static List<String> GetWholeVowels()
        {
            return CharToStrList(DevanagariVowels) ;
        }

        public static List<String> GetDependentVowels()
        {
            return CharToStrList(DevanagariDependentVowels);
        }

        public static List<String> GetDevanagariNumbers()
        {
            return CharToStrList(DevanagariNumbers);
        }

        public static List<String> GetJoiningSymbols()
        {
            return CharToStrList(SpecialJoiningSymbols);
        }

        public static List<String> GetSpecialSymbols()
        {
            return CharToStrList(SpecialSymbols);
        }

        public static String GetHalanta()
        {
            return Halanta[0].ToString();
        }

        public static String GetNonJoiningCharacter()
        {
            return NonJoinChar[0].ToString();
        }

        public static String GetJoiningCharacter()
        {
            return JoinChar[0].ToString();
        }

        private static List<String> CharToStrList(List<Char> data)
        {
            return data.Select(ch => ch.ToString()).ToList<String>();
        }
    }
}
