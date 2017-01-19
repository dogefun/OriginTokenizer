using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginTokenizer
{
    //Remember add default token
    class Scanner
    {
        //attritude
        private int[,] table;
        private int[] hash;
        private List<Regex> regexList;
        private List<DFAState> states;
        //stateControl
        private int nowState = 0;
        private int lastState = -1;
        private int readIndex = 0;
        private string source;
        //Scanner Setting
        private List<Regex> endState;
        private bool spaceEnd;
        public Scanner(ScannerInfo info)
        {
            table = info.DfaTable;
            hash = info.Hash;
            regexList = info.RegexList;
            states = info.States;  
        }
        
        public void SetSource(string s)
        {
            readIndex = 0;
            source = s;
        }

        public Token Read()
        {
            nowState = 0;
            lastState = -1;
            string input = "";
            while (true)
            {
                if(readIndex >= source.Length)
                {
                    if (states[nowState].isEndState)
                    {
                        var token = new Token();
                        token.Describtion = states[nowState].Describtion;
                        token.value = input;
                        return token;
                    }

                    var t = Token.EndOfSourceToken;
                    t.value = input;
                    return t;
                }

                input += source[readIndex];
                var x = hash[source[readIndex]];
                Go(x);

                readIndex++;

                if (nowState < 0)
                {
                    readIndex--;
                    if (states[lastState].isEndState)
                    {
                        var token = new Token();
                        token.Describtion = states[lastState].Describtion;
                        token.value = input.Substring(0,input.Length - 1);
                        return token;
                    }
                }
            }
            
            return null;
        }

        private void Go(int statement)
        {
            if (nowState < 0)
            {
                return;
            }
            var x = table[nowState, statement];
            lastState = nowState;
            nowState = x;
        }
        //public void SetEnd
    }
}
