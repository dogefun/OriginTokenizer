using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OriginTokenizer
{
    //需要高效保证lead无重的方法
    class NFAEdge
    {
        public NFAState lead;
        public int statement = 0;

        public NFAEdge()
        {

        }
        public NFAEdge(NFAState state)
        {
            lead = state;
        }

        public NFAEdge(NFAState state, int statement)
        {
            lead = state;
            this.statement = statement;
        }
    }
}
