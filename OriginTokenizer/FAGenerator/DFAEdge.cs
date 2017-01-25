using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
