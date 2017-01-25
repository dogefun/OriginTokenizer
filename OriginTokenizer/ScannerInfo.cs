using System;
using System.Collections.Generic;
using System.Linq;

namespace OriginTokenizer
{
    /// <summary>
    /// 用于将DFA模型转化为相应的二维数组以及其他数据
    /// </summary>
    public class ScannerInfo
    {
        private DFAModel dfa;
        private int[] hash;
        private int[,] dfaTable;
        private List<Token> faState;
        private bool isGenerate = false;
        private int index = 1;
        internal int[] Hash { get {if(isGenerate) return hash;return null; } }
        internal List<Token> States { get { if (isGenerate) return faState; return null; } }
        internal int[,] DfaTable { get { if (isGenerate) return dfaTable; return null; } }
        public ScannerInfo()
        {
            dfa = new DFAModel();
        }

        /// <summary>
        /// add new regex to dfa to identify text
        /// </summary>
        /// <param name="regex"></param>
        public void AddRegex(RegularExpression regex)
        {
            if (isGenerate)
                throw new Exception("Cant operate AddRegex after data generated");
            regex.index = index++;
            dfa.SetRegularExpression(regex);
        }
        public ScannerInfo GenerateData()
        {
            if (isGenerate)
                return this;
            dfa.CreateDFAModel();

            faState = new List<Token>();
            
            for (int i = 0;i < dfa.DFAList.Count; i++)
            {
                faState.Add(null);
                if (dfa.DFAList[i].isEndState)
                {
                    faState[i] = new Token();
                    faState[i].Describtion = dfa.DFAList[i].Describtion;
                    faState[i].status = dfa.DFAList[i].index;
                }
            }

            var inputSet = new List<int>();
            dfaTable = new int[faState.Count,256];

            //too stupid way to init inputSet
            for(int i = 0;i < dfa.DFAList.Count; i++)
            {
                for(int j = 0;j < 256; j++)
                {
                    dfaTable[i, j] = -1;
                }
            }
            //init hash table
            hash = new int[256];
            for (int i = 0; i < 256; i++)
                hash[i] = -1;

            for (int i = 0; i < faState.Count; i++)
            {
                var t = dfa.DFAList[i];
                foreach (var x in t.Lead)
                {
                    if (!inputSet.Contains(x.statement))
                    {
                        inputSet.Add(x.statement);
                    }
                    dfaTable[i, x.statement] = x.lead.Id;
                }
            }

            //trying to shorten with the method below
            isGenerate = true;
            compressDFA(inputSet);
            return this;
        }

        //shorten new dfaTable and hash
        //HACK: Poor performance
        private void compressDFA(List<int> input)
        {
            var list = new List<List<int>>();

            foreach (var x in input)
            {
                var l = new List<int>();
                for(int i = 0;i < dfa.DFAList.Count; i++)
                {
                    l.Add(dfaTable[i, x]);
                }
                
                var index = list.FindIndex(a => l.Except(a).Count() == 0 && a.Except(l).Count() == 0);
                if (index == -1)
                {
                    list.Add(l);
                    hash[x] = list.Count - 1;
                }
                else
                {
                    hash[x] = index;
                }
            }

            dfaTable = new int[faState.Count, list.Count];


            for (int i = 0; i < dfa.DFAList.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    dfaTable[i, j] = list[j][i];
                }
            }
        }

        private static bool isEqual(List<int> a,List<int> b)
        {
            return b.Except(a).Count() == 0 && a.Except(b).Count() == 0;
        }
    }
}
