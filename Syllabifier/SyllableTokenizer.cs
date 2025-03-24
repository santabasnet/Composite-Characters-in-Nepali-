using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Syllabifier
{
    /// <summary>
    /// Represents all the states in the state machine.
    /// </summary>
    enum State
    {
        ZERO, ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, UNICODE_FINAL_STATE
    }

    public class SyllableTokenizer
    {
        public static List<String> findAllBoundaries(String wordText)
        {
            return new UnicodeTokenizer(wordText).GetGroupedCharacters().Select(token => token.GetName()).ToList<String>();
        }
    }

    /// <summary>
    /// Main Unicode tokenizer class, this implements the automaton to
    /// group the unicode devanagari text based on syllable boundaries.
    /// </summary>
    class UnicodeTokenizer
    {
        /// <summary>
        /// Unicode Tokenizer Data, assigned in static scope.
        /// </summary>
        private static readonly TokenizerData TOKENIZER_DATA = new TokenizerData();

        private String sourceText;
        private List<GroupToken> tokensList;

        /// <summary>
        /// Default constructor, it overloads the source text and call tokenize functionality internally.
        /// </summary>
        /// <param name="sourceText"></param>
        public UnicodeTokenizer(String sourceText)
        {
            this.sourceText = sourceText;
            this.tokensList = new List<GroupToken>();
            this.tokenize();
        }

        /// <summary>
        /// Returns the list of tokens i.e. the output of the tokenizer.
        /// </summary>
        /// <returns></returns>
        public List<GroupToken> GetGroupedCharacters()
        {
            //Console.WriteLine(this.tokensList);
            //Console.WriteLine(this.tokensList.Count);
            return this.tokensList;
        }

        /**
         * Perform Tokenization. 
         */
        private void tokenize()
        {
            InitializeTokenizer();
            GroupToken token = GetNextToken();
            while (token != null)
            {
                tokensList.Add(token);
                token = GetNextToken();
            }
        }

        //Token data
        private int sourceIndex;
        private State currentState;
        String currentTokenStr;
        String tokenType;

        private void InitializeTokenizer()
        {
            this.sourceIndex = 0;
        }

        private GroupToken GetNextToken()
        {
            //Default boundary condition
            if (this.sourceIndex >= this.sourceText.Length) return null;

            this.currentTokenStr = "";
            this.currentState = State.ONE;
            this.tokenType = TokenNames.ELSE;

            for (; this.sourceIndex < this.sourceText.Length;)
            {
                String chStr = GetNextChar() + "";
                //Console.WriteLine("char :" + chStr + "\ttokenstr -> " + currentTokenStr + "\tstate :" + currentState + "\ttokentype :" + tokenType);

                switch (this.currentState)
                {
                    case State.ONE:
                        if (TOKENIZER_DATA.IsWholeVowel(chStr))
                        {
                            tokenType = TokenNames.WV;
                            this.currentState = State.SIX;
                        }
                        else if (TOKENIZER_DATA.IsWholeConsonant(chStr))
                        {
                            tokenType = TokenNames.WC;
                            this.currentState = State.TWO;
                        }
                        else if (TOKENIZER_DATA.IsDevanagariNumber(chStr))
                        {
                            tokenType = TokenNames.NUM;
                            this.currentState = State.FIVE;
                        }
                        else if (TOKENIZER_DATA.IsPunctuationMark(chStr))
                        {
                            tokenType = TokenNames.PCN;
                            this.currentState = State.SEVEN;
                        }
                        else if (TOKENIZER_DATA.IsSpecialSymbol(chStr))
                        {
                            tokenType = TokenNames.SPS;
                            this.currentState = State.UNICODE_FINAL_STATE;
                        }
                        else
                        {
                            this.currentState = State.UNICODE_FINAL_STATE;
                        }
                        currentTokenStr += chStr;
                        break;

                    case State.TWO:
                        if (TOKENIZER_DATA.IsJoinableSymbol(chStr))
                        {
                            currentTokenStr += chStr;
                            tokenType += TokenNames.CONNECTOR + TokenNames.JS;
                            this.currentState = State.UNICODE_FINAL_STATE;
                        }
                        else if (TOKENIZER_DATA.IsHalanta(chStr))
                        {
                            currentTokenStr += chStr;
                            tokenType += TokenNames.CONNECTOR + TokenNames.HLN;
                            this.currentState = State.THREE;
                        }
                        else if (TOKENIZER_DATA.IsDependentVowel(chStr))
                        {
                            currentTokenStr += chStr;
                            tokenType += TokenNames.CONNECTOR + TokenNames.DV;
                            this.currentState = State.FOUR;
                        }
                        else if (TOKENIZER_DATA.IsPunctuationMark(chStr))
                        {
                            UnGetCurrentChar();
                            this.currentState = State.SEVEN;
                        }
                        else
                        {
                            this.currentState = State.UNICODE_FINAL_STATE;
                            UnGetCurrentChar();
                        }
                        break;

                    case State.THREE:
                        if (TOKENIZER_DATA.IsWholeConsonant(chStr))
                        {
                            currentTokenStr += chStr;
                            tokenType += TokenNames.CONNECTOR + TokenNames.WC;
                            currentState = State.TWO;
                        }
                        else if (TOKENIZER_DATA.IsZWNJ(chStr))
                        {
                            currentTokenStr += chStr;
                            currentState = State.UNICODE_FINAL_STATE;
                            tokenType += TokenNames.CONNECTOR + TokenNames.ZWNJ;
                        }
                        else if (TOKENIZER_DATA.IsZWJ(chStr))
                        {
                            currentTokenStr += chStr;
                            currentState = State.UNICODE_FINAL_STATE;
                            tokenType += TokenNames.CONNECTOR + TokenNames.ZWJ;
                        }
                        else if (TOKENIZER_DATA.IsPunctuationMark(chStr))
                        {
                            UnGetCurrentChar();
                            this.currentState = State.SEVEN;
                        }
                        else
                        {
                            this.currentState = State.UNICODE_FINAL_STATE;
                            UnGetCurrentChar();
                        }
                        break;

                    case State.FOUR:
                        if (TOKENIZER_DATA.IsJoinableSymbol(chStr))
                        {
                            tokenType += TokenNames.CONNECTOR + TokenNames.JS;
                            currentTokenStr += chStr;
                        }
                        else if (TOKENIZER_DATA.IsPunctuationMark(chStr))
                        {
                            UnGetCurrentChar();
                            this.currentState = State.SEVEN;
                        }
                        else
                        {
                            UnGetCurrentChar();
                        }
                        this.currentState = State.UNICODE_FINAL_STATE;
                        break;

                    case State.FIVE:
                        if (TOKENIZER_DATA.IsDevanagariNumber(chStr))
                        {
                            this.currentState = State.FIVE;
                            currentTokenStr += chStr;
                        }
                        else if (TOKENIZER_DATA.IsPunctuationMark(chStr))
                        {
                            this.currentState = State.SEVEN;
                            UnGetCurrentChar();
                        }
                        else
                        {
                            this.currentState = State.UNICODE_FINAL_STATE;
                            UnGetCurrentChar();
                        }
                        break;

                    case State.SIX:
                        if (TOKENIZER_DATA.IsJoinableSymbol(chStr))
                        {
                            tokenType += TokenNames.CONNECTOR + TokenNames.JS;
                            currentTokenStr += chStr;
                        }
                        else if (TOKENIZER_DATA.IsPunctuationMark(chStr))
                        {
                            UnGetCurrentChar();
                            this.currentState = State.SEVEN;
                        }
                        else UnGetCurrentChar();

                        this.currentState = State.UNICODE_FINAL_STATE;
                        break;

                    case State.SEVEN:
                        if (TOKENIZER_DATA.IsPunctuationMark(chStr))
                        {
                            currentTokenStr += chStr;
                            tokenType += TokenNames.CONNECTOR + TokenNames.PCN;
                            this.currentState = State.SEVEN;
                        }
                        else UnGetCurrentChar();
                        break;

                    default:
                        this.currentState = State.UNICODE_FINAL_STATE;
                        this.tokenType += TokenNames.ELSE;
                        this.currentTokenStr += chStr;
                        break;
                }
                if (this.currentState == State.UNICODE_FINAL_STATE || this.currentState == State.SEVEN) break;
            }
            return String.IsNullOrEmpty(currentTokenStr) ? null : new GroupToken(tokenType, currentTokenStr);
        }

        private char GetNextChar()
        {
            return this.sourceText[this.sourceIndex++];
        }

        private void UnGetCurrentChar()
        {
            this.sourceIndex--;
        }

    }
    /// <summary>
    /// Group definition of the devanagari syllable unit, a token.
    /// </summary>
    class GroupToken : IComparable<GroupToken>
    {
        private String type, name;
        private ArrayList typeList;

        public GroupToken(String type, String name)
        {
            this.type = type;
            this.name = name;
            this.typeList = new ArrayList();
        }

        public GroupToken(String type, String name, ArrayList typeList)
        {
            this.type = type;
            this.name = name;
            this.typeList = typeList;
        }

        public static GroupToken empty()
        {
            return new GroupToken(Global.EMPTY_STRING, Global.EMPTY_STRING);
        }

        public bool isEmpty()
        {
            return String.IsNullOrEmpty(GetType()) || String.IsNullOrEmpty(GetName());
        }

        public bool nonEmpty()
        {
            return !isEmpty();
        }

        public new String GetType()
        {
            return this.type;
        }

        public void SetType(String type)
        {
            this.type = type;
        }

        public void SetName(String name)
        {
            this.name = name;
        }

        public String GetName()
        {
            return this.name;
        }

        public bool IsStartedWith(String tokenPrefix)
        {
            return GetName().StartsWith(tokenPrefix);
        }

        public bool IsEndedWith(String tokenSuffix)
        {
            return GetName().EndsWith(tokenSuffix);
        }

        public bool IsTypeStartedWith(String typePrefix)
        {
            return GetType().StartsWith(typePrefix);
        }

        public bool IsTypeEndedWith(String typeSuffix)
        {
            return GetType().EndsWith(typeSuffix);
        }

        public bool HasTokenType(String tokenStr)
        {
            return GetType().Contains(tokenStr);
        }

        public int GetTokenIndex(String typeStr)
        {
            return this.typeList.IndexOf(typeStr);
        }

        public int GetLastTokenIndex(String typeStr)
        {
            return this.typeList.LastIndexOf(typeStr);
        }

        public String IndexOfTokenType(int typeIndex)
        {
            try
            {
                return this.typeList[typeIndex].ToString();
            }
            catch (Exception e)
            {
                return Global.EMPTY_STRING;
            }
        }

        public String RemoveType(String typeStr)
        {
            if (typeStr.Length < GetType().Length) return GetType().Substring(typeStr.Length + 1);
            else return Global.EMPTY_STRING;
        }

        public String RemoveTypeUpto(int n)
        {
            return GetType().Substring(StringUtils.OrdinalIndexOf(type, TokenNames.CONNECTOR, n) + 1);
        }

        public String GetTypeUpto(int n)
        {
            return GetType().Substring(0, StringUtils.OrdinalIndexOf(type, TokenNames.CONNECTOR, n));
        }

        public String RemoveLastType(String typeStr)
        {
            if (GetType().Length > typeStr.Length)
            {
                return GetType().Substring(0, GetType().Length - (typeStr.Length + 1));
            }
            return Global.EMPTY_STRING;
        }

        public String PreviousToken(int rhfPosition)
        {
            try
            {
                return this.typeList[rhfPosition - 1].ToString();
            }
            catch (Exception ee)
            {
                return Global.EMPTY_STRING;
            }
        }

        public String MoveNthTokenTypeToLast(int tokPosition)
        {
            ArrayList tmpTypeList = new ArrayList(this.typeList);
            String typeStr = tmpTypeList[tokPosition].ToString();
            tmpTypeList.Remove(tokPosition);
            tmpTypeList.Add(typeStr);
            String resultTypeStr = "";
            foreach (String tStr in tmpTypeList)
            {
                resultTypeStr += tStr + TokenNames.CONNECTOR;
            }
            return resultTypeStr.Substring(0, resultTypeStr.Length - 1);
        }

        public int getNextPosition(int index)
        {
            try
            {
                if (this.typeList[index + 1].Equals(TokenNames.RV))
                {
                    try
                    {
                        if (this.typeList[index + 2].Equals(TokenNames.TV))
                        {
                            return (2 + index);
                        }
                    }
                    catch (Exception ee)
                    {
                        return (1 + index);
                    }
                }
                return index;
            }
            catch (Exception ee)
            {
                return index;
            }
        }

        public int CompareTo(GroupToken other)
        {
            return this.GetType().CompareTo(other.GetType());
        }

        public override string ToString()
        {
            return GetName() + "{" + GetType() + "}";
        }

        public override bool Equals(object newObject)
        {
            if (newObject == null) return false;
            else return GetType().CompareTo(((GroupToken)newObject).GetType()) == 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public GroupToken GetRHFAltered(int rhfPosition)
        {
            ArrayList tTypeList = new ArrayList(this.typeList);
            String removedType = tTypeList[rhfPosition - 1].ToString();
            tTypeList.Remove(rhfPosition - 1);
            tTypeList.Insert(rhfPosition, removedType);
            String tName = GetName().Substring(0, rhfPosition - 1)
                    + GetName().Substring(rhfPosition, rhfPosition + 1)
                    + GetName().Substring(rhfPosition - 1, rhfPosition);
            return new GroupToken(String.Join("_", tTypeList), tName, tTypeList);
        }

    }
}
