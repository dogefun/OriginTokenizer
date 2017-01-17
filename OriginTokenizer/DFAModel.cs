using OriginTokenizer.Utils;
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
        public List<NFAModel> NFACollection;
        public List<DFAState> DFAList;

        public DFAModel()
        {
            NFACollection = new List<NFAModel>();
            DFAList = new List<DFAState>();
        }
        public void SetRegularExpression(Regex regex)
        {
            var nfa = regex.DefineedNFAModel;
            if (NFACollection.Contains(nfa))
                return;

            NFACollection.Add(nfa);
        }

        public void CreateDFAModel()
        {
            var finalNFA = new NFAModel();
            var tmpState = new NFAState();
            finalNFA.entryEdge = new NFAEdge(tmpState);
            foreach(var x in NFACollection)
            {
                tmpState.AddEdge(x.entryEdge);
            }

            finalNFA.RenameStates(1);

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

        }

        public static DFAModel CreateDFAModel(NFAModel nfa)
        {
            DFAModel model = new DFAModel();
            model.NFACollection.Add(nfa);
            model.CreateDFAModel();
            return model;
        }

        public string Properites()
        {
            string a = "Contain:\n";
            foreach(var x in DFAList)
            {
                a += x.Id.ToString() + "\n";
                foreach(var y in x.ContainStates)
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
