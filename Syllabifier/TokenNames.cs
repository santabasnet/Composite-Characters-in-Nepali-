using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syllabifier
{
    class TokenNames
    {
        public static readonly String TV = "TV"; //Top Vowel
        public static readonly String RV = "RV"; //Right Vowel

        public static readonly String CONNECTOR = "_";

        /**
        * Special Symbol.
        */
        public static readonly String SPS = "SPS";
    
        /**
         * Whole consonant.
         */
        public static readonly String WC = "WC";
    
        /**
         * Joining Symbol.
         */
        public static readonly String JS = "JS";
    
        /**
         * Dependent Vowel.
         */
        public static readonly String DV = "DV";
    
        /**
         * Halanta.
         */
        public static readonly String HLN = "HLN";
    
        /**
         * Zero with non-joiner.
         */
        public static readonly String ZWNJ = "ZWNJ";
    
        /**
         * Zero with joiner.
         */
        public static readonly String ZWJ = "ZWJ";
    
        /**
         * Number token.
         */
        public static readonly String NUM = "NUM";
    
        /**
         * Whole Vowel.
         */
        public static readonly String WV = "WV";
    
        /**
         * Punctuation Marks.
         */
        public static readonly String PCN = "PCN";
    
        /**
         * Others token.
         */
        public static readonly String ELSE = "ELSE";

    }
}
