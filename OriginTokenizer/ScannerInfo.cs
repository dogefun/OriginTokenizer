using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginTokenizer
{
    class ScannerInfo
    {
        struct leadInfoSet
        {
            internal int lead;
            internal List<int> set;

            internal leadInfoSet(int l)
            {
                lead = l;
                set = new List<int>();
            }
        }

        private List<RegularExpression> regexList;
        private DFAModel dfa;
        private int[] hash;             //very simple hash used to shorten dfa table
        private int[,] dfaTable;
        private List<DFAState> faState;
        private bool ready = false;

        public int[] Hash { get { if (ready) return hash; return null; } }
        public List<DFAState> States { get { if (ready) return faState; return null; } }
        public int[,] DfaTable { get { if (ready) return dfaTable; return null; } }
        public List<RegularExpression> RegexList { get { return regexList; } }
        public ScannerInfo()
        {
            regexList = new List<RegularExpression>();
            dfa = new DFAModel();
        }

        public void AddRegex(RegularExpression regex)
        {
            if (!regexList.Contains(regex))
                regexList.Add(regex);
            dfa.SetRegularExpression(regex);
        }
        public void setSkipRegex(RegularExpression regex)
        {
            if (!regexList.Contains(regex))
                regexList.Add(regex);
            dfa.SetRegularExpression(regex);
        }
        public ScannerInfo CreateInfo()
        {
            dfa.CreateDFAModel();
            faState = dfa.DFAList;
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
            //Priority
            for (int i = 0; i < faState.Count; i++)
            {
                var t = dfa.DFAList[i];
                foreach(var x in t.Lead)
                {
                    if (!inputSet.Contains(x.statement))
                    {
                        inputSet.Add(x.statement);
                    }
                    dfaTable[i, x.statement] = x.lead.Id;
                }
            }

            //trying to shorten with the method below
            compressDFA(inputSet);
            ready = true;
            return this;
        }

        //make new dfaTable and hash
        //complete,maybe can combine with createInfo
        private void compressDFA(List<int> input)
        {
            var list = new List<List<int>>();

            hash = new int[256];
            for (int i = 0; i < 256; i++)
                hash[i] = -1;

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
    }
}
