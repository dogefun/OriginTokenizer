using System.Collections.Generic;

namespace OriginTokenizer
{
    /// <summary>
    /// 用于扫描字符串并分词 
    /// </summary>
    public class Scanner
    {
        //attritude
        private int[,] table;
        private int[] hash;
        private List<int> skipList;
        private List<Token> states;
        //stateControl
        private int nowState = 0;
        private int lastState = -1;
        private int readIndex = 0;
        private string source;

        public Scanner(ScannerInfo info)
        {
            table = info.DfaTable;
            hash = info.Hash;
            skipList = new List<int>();
            states = info.States;
        }
        /// <summary>
        /// Skip Token
        /// Such as space or anything use to devide words
        /// Regex must define inside ScannerInfo
        /// </summary>
        /// <param name="regex"></param>
        public void SetSkipTokenRegex(RegularExpression regex)
        {

            skipList.Add(regex.index);
        }
        /// <summary>
        /// Set source text 
        /// </summary>
        /// <param name="s">source text</param>
        public void SetSource(string s)
        {
            readIndex = 0;
            source = s;
        }
        /// <summary>
        /// Scan first/next word 
        /// </summary>
        /// <returns>Token contains scanned word and his means(describtion)</returns>
        public Token Read()
        {
            nowState = 0;
            lastState = -1;
            string input = "";
            while (true)
            {
                //readIndex touch bound
                if(readIndex > source.Length)
                {
                    var t = Token.EndOfSourceToken;
                    t.value = input;
                    return t;
                }
                if (readIndex == source.Length)
                {
                    readIndex++;
                    if (states[nowState] != null && !skipList.Contains(states[nowState].Index))
                    {
                        var token = states[nowState].Clone() as Token;
                        //token.Describtion = states[nowState].Describtion;
                        //token.status = states[nowState].Index;
                        token.value = input;
                        token.startPosition = readIndex - 1 - input.Length;
                        return token;
                    }
                    continue;
                }

                input += source[readIndex];
                var x = hash[source[readIndex]];
                Go(x);
                readIndex++;

                //DFA dead end
                if (nowState < 0)
                {
                    readIndex--;
                    if (lastState >= 0 && states[lastState] != null)
                    {
                        if (skipList.Contains(states[lastState].Index))
                        {
                            return Read();
                        }
                        //var token = new Token();
                        var token = states[lastState].Clone() as Token;
                        //token.Describtion = states[lastState].Describtion;
                        //token.status = states[lastState].index;
                        token.value = input.Substring(0, input.Length - 1);
                        token.startPosition = readIndex - input.Length + 1;

                        return token;
                    }
                    var t = Token.ErrorToken;
                    t.value = input + source.Substring(readIndex + 1);
                    return t;
                }
            }
            
        }
        public List<Token> ReadAll()
        {
            var list = new List<Token>();
            while (true)
            {
                var token = Read();
                if (token.status < 0)
                    break;
                list.Add(token);
            }
            return list;
        }
        /// <summary>
        /// go to next statement
        /// </summary>
        /// <param name="statement">the statement go</param>
        private void Go(int statement)
        {
            if (nowState < 0)
            {
                return;
            }
            if(statement < 0)
            {
                nowState = -1;
                return;
            }
            var x = table[nowState, statement];
            lastState = nowState;
            nowState = x;
        }
    }
}
