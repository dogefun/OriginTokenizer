﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginTokenizer
{
    class NFAState
    {
        public int id;
        public bool isEndState;
        private List<NFAEdge> lead = new List<NFAEdge>();

        public List<NFAEdge> Lead
        {
            get
            {
                return new List<NFAEdge>(lead);
            }
        }

        public NFAState() { }
        public NFAState(int id) {
            this.id = id;
        }

        public void AddEdge(NFAEdge e)
        {
            lead.Add(e);
        }

        public void AddEdgeTo(NFAState target)
        {
            lead.Add(new NFAEdge(target));
        }

        public void RebuildStates(List<NFAState> list)
        {
            if (list.Contains(this))
                return;

            list.Add(this);

            if (this.lead.Count == 0)
            {
                return;
            }
            else
            {
                foreach(var x in lead)
                {
                    x.lead.RebuildStates(list);
                }
            }
        }
    }
}
