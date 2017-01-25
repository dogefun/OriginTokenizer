using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OriginTokenizer
{
    class DFAState
    {
        private List<NFAState> states;
        private List<NFAEdge> edges;
        private List<DFAEdge> lead = new List<DFAEdge>();

        internal int index;
        internal bool isEndState;
        internal string describtion;

        public string Describtion
        {
            get
            {
                if (describtion == null)
                    return "";
                return describtion;
            }
        }
        public int Id
        {
            get;set;
        }
        public List<NFAState> States { get { return states; } }
        public List<NFAEdge> Edges{ get { return edges; } }
        public List<DFAEdge> Lead { get { return lead; } }
        public DFAState()
        {
            states = new List<NFAState>();
            edges = new List<NFAEdge>(); 
        }

        public DFAState(DFAState s)
        {
            states = new List<NFAState>(s.states);
            edges = new List<NFAEdge>(s.edges);
        }

        public DFAState(NFAState s)
        {
            states = new List<NFAState>();
            edges = new List<NFAEdge>();

            AddState(s);  
        }

        //tested
        public DFAState E_closure(int e)
        {
            var newstate = new DFAState();
            foreach(var x in edges)
            {
                if(x.statement == e)
                {
                    newstate.AddState(x.lead);
                }
            }
            return newstate;
        }

        public void LeadTo(DFAState s,int e)
        {
            lead.Add(new DFAEdge(s, e));
        }
        //maybe need to be faster
        private void AddState(NFAState s)
        {
            if (states.Contains(s))
                return;
            states.Add(s);
            edges = edges.Union(s.Lead).ToList();
            for (int i = 0; i < edges.Count; i++)
            {
                var x = edges[i];
                if (x.statement == 0)
                {
                    AddState(x.lead);
                    edges.Remove(x);
                    i--;
                }
            }
        }

        public string Properites()
        {
            string log = "has below states:\n";
            foreach(var x in states)
            {
                log += x.id.ToString() + "\n";
            }
            log += "\nhas lead to :\n";
            foreach (var x in edges)
            {
                log += x.statement.ToString() + "  " +x.lead.id.ToString() + "\n";
            }
            return log;
        }

        public bool Contains(NFAState s)
        {
            return states.Contains(s);
        }
        public bool isEqual(DFAState s)
        {
            return states.Except(s.states).Count() == 0 && s.states.Except(states).Count() == 0;
        }
    }
}
