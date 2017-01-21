using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginTokenizer
{
    class DFAModel
    {
        public DFAEdge entryEdge;
        public List<RegularExpression> regexCollection;
        public List<DFAState> DFAList;

        public DFAModel()
        {
            regexCollection = new List<RegularExpression>();
            DFAList = new List<DFAState>();
        }
        public void SetRegularExpression(RegularExpression regex)
        {
            if (regexCollection.Contains(regex))
                return;

            regexCollection.Add(regex);
        }

        public void CreateDFAModel()
        {
            var finalNFA = new NFAModel();
            var tmpState = new NFAState(0);
            finalNFA.entryEdge = new NFAEdge(tmpState);
            var index = 1;
            foreach(var regex in regexCollection)
            {
                var x = regex.NFAModel;
                tmpState.AddEdge(x.entryEdge);
                index = x.RenameStates(index);
            }

            //make first dfaset
            DFAState first = new DFAState(tmpState);
            int id = 0;
            first.Id = id++;
            DFAList.Add(first);

            for(int i = 0;i < DFAList.Count; i++)
            {
                var list = new List<int>();
                //possible move
                foreach(var x in DFAList[i].Edges)
                {
                    if (!list.Contains(x.statement))
                        list.Add(x.statement);
                }

                foreach (var x in list)
                {
                    var lead = DFAList[i].E_closure(x);

                    var have = false;
                    foreach (var u in DFAList)
                    {
                        if (u.isEqual(lead))
                        {
                            DFAList[i].LeadTo(u, x);
                            have = true;
                            break;
                        }
                    }
                    if (have)
                    {
                        continue;
                    }
                    lead.Id = id++;
                    DFAList[i].LeadTo(lead, x);

                    DFAList.Add(lead);
                }
            }

            //must be very very slow
            for(int i = regexCollection.Count - 1;i >= 0; i--)
            {
                var op = regexCollection[i];
                var nfa = op.NFAModel;
                foreach (var x in DFAList)
                {
                    if (x.Contains(nfa.tailState))
                    {
                        x.isEndState = true;
                        x.describtion = op.Describtion;
                    }
                }
            }
        }

        public static DFAModel CreateDFAModel(RegularExpression regex)
        {
            DFAModel model = new DFAModel();
            model.regexCollection.Add(regex);
            model.CreateDFAModel();
            return model;
        }

        public string Properites()
        {
            string a = "Contain:\n";
            foreach(var x in DFAList)
            {
                a += x.Id.ToString() + "\n";
                if (x.isEndState)
                {
                    a += "End Of ";
                    a += x.describtion;
                    a += "\n";
                }
                foreach(var y in x.States)
                {
                    a += y.id.ToString() + " ";
                }
                a += "\n";
                foreach (var y in x.Lead)
                {
                    a += "    " + y.statement.ToString() + "  lead to  " + y.lead.Id.ToString() + "\n";
                }
            }
            return a;
        }
    }
}
