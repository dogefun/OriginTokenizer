using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Regex is used to create NFA model using RegularExpression rule
namespace OriginTokenizer
{
    public class RegularExpression 
    {
        private string regexDescribtion;
        private NFAModel nfa;
        internal int index;

        public int Index
        {
            get
            {
                return index;
            }
        }
        public string Describtion
        {
            get
            {
                return regexDescribtion;
            }
            set
            {
                if(regexDescribtion == null || regexDescribtion == "")
                {
                    regexDescribtion = value;
                }
            }
        }
        internal NFAModel NFAModel
        {
            get
            {
                return nfa;
            }
        }
        public RegularExpression() { }
        public RegularExpression(string regexDescribtion)
        {
            this.regexDescribtion = regexDescribtion;
        }

        //can only contain value
        //97-122,65-90,48-57
        //a-z，A-Z，1-9
        public static RegularExpression DefineRange(char x, char y)
        {
            if(x > y)
            {
                var t = x;
                x = y;
                y = x;
            }

            RegularExpression r = new RegularExpression();
            var model = r.nfa = new NFAModel();
            NFAState s = new NFAState(0);
            model.tailState = new NFAState(1);
            model.entryEdge = new NFAEdge(s);

            for(int i = x;i <= y; i++)
            {
                s.AddEdge(new NFAEdge(model.tailState, i));
            }

            return r;
        }

        //basisOp
        //tested
        public RegularExpression DefineLiteral(string input)
        {
            NFAModel model = new NFAModel();
            NFAState s = new NFAState(0);
            
            model.entryEdge = new NFAEdge(s, 0);
            for (int i = 0; i < input.Length; i++)
            {
                NFAState state = new NFAState(i + 1);
                s.AddEdge(new NFAEdge(state, input[i]));
                s = state;
            }

            model.tailState = new NFAState(s.id + 1);
            s.AddEdgeTo(model.tailState);
            nfa = model;
            return this;
        }

        public static RegularExpression CreateWithLiteral(string input)
        {
            RegularExpression r = new RegularExpression();
            r.DefineLiteral(input);
            return r;
        }
       

        //tested
        public RegularExpression Union(RegularExpression right)
        {
            var left = this;
            NFAModel model = new NFAModel();
            NFAState s = new NFAState(0);
            model.entryEdge = new NFAEdge(s, 0);

            s.AddEdge(left.nfa.entryEdge);
            s.AddEdge(right.nfa.entryEdge);

            model.tailState = new NFAState();
            left.nfa.tailState.AddEdgeTo(model.tailState);
            right.nfa.tailState.AddEdgeTo(model.tailState);

            nfa = model;
            return this;
        }

       

        //tested
        public RegularExpression Concat(RegularExpression right)
        {
            NFAModel model = this.nfa;
            model.tailState.AddEdge(right.nfa.entryEdge);
            model.tailState = right.nfa.tailState;
            return this;
        }

        public RegularExpression KleeneStar()
        {
            NFAModel model = new NFAModel();
            model.tailState = new NFAState();
            model.entryEdge = new NFAEdge(model.tailState, 0);

            nfa.tailState.AddEdgeTo(model.tailState);
            model.tailState.AddEdge(nfa.entryEdge);

            nfa = model;
            return this;
        }

        public static RegularExpression DefineKleeneStar(string input)
        {
            return CreateWithLiteral(input).KleeneStar();
        }

        public static RegularExpression DefineConcat(RegularExpression left, RegularExpression right)
        {
            left.Concat(right);
            return left;
        }

        public static RegularExpression DefineUnion(RegularExpression left, RegularExpression right)
        {
            left.Union(right);
            return left;
        }
    }
}
