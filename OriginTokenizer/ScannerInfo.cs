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

        private List<Regex> regexList;
        private DFAModel dfa;
        private int[] hash;             //very simple hash used to shorten dfa table
        private int[,] dfaTable;
        private List<DFAState> faState;
        private bool ready = false;

        public int[] Hash { get { if (ready) return hash; return null; } }
        public List<DFAState> States { get { if (ready) return faState; return null; } }
        public int[,] DfaTable { get { if (ready) return dfaTable; return null; } }
        public List<Regex> RegexList { get { return regexList; } }
        public ScannerInfo()
        {
            regexList = new List<Regex>();
            dfa = new DFAModel();
        }

        public void AddRegex(Regex regex)
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
        //uncomplete,maybe later
        private void compressDFA(List<int> input)
        {
            hash = new int[256];
            for (int i = 0; i < 256; i++)
                hash[i] = i;
            //var equClass = new List<leadInfoSet>();
            //foreach (var x in inputSet)
            //{
            //    var e = new leadInfoSet(x);
            //    for (int i = 0; i < dfa.DFAList.Count; i++)
            //    {
            //        e.set.Add(dfaTable[0, x]);
            //    }
            //    equClass.Add(e);
            //}
        }
    }
}
