using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginTokenizer
{
    class DFAEdge
    {
        internal DFAState lead;
        internal int statement = 0;

        public DFAEdge()
        {

        }
        public DFAEdge(DFAState state)
        {
            lead = state;
        }

        public DFAEdge(DFAState state, int statement)
        {
            lead = state;
            this.statement = statement;
        }
    }
}
