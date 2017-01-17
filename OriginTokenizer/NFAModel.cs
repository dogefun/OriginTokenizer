using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginTokenizer
{
    class NFAModel
    {
        public NFAEdge entryEdge;
        public List<NFAState> states;
        public NFAState tailState;

        public int Count
        {
            get
            {
                return states.Count;
            }
        }

        public NFAModel()
        {
            states = new List<NFAState>();
        }

        public int RenameStates(int input)
        {
            states.Clear();
            entryEdge.lead.RebuildStates(states);
            foreach (var x in states)
            {
                x.id = input++;
            }
            return input;
        }

        public string ShowStatesS()
        {
            string res = "";

            foreach (var x in states)
            {
                res += String.Format("id {0}  lead to:\n",x.id);
                foreach(var y in x.Lead)
                {
                    res += String.Format("    value {1} lead to {0}\n", y.lead.id,y.statement);
                }
            }

            return res;
        }
    }
}
