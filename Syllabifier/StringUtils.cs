using System;

namespace Syllabifier
{
    class StringUtils
    {
        public static int OrdinalIndexOf(String source, String subStr, int n)
        {
            int pos = -1;
            do
            {
                pos = source.IndexOf(subStr, pos + 1);
            } while (n-- > 0 && pos != -1);
            return pos;
        }
    }
}
