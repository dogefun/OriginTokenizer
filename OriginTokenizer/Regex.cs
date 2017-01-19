using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Regex is used to create NFA model using RegularExpression rule
namespace OriginTokenizer
{
    class Regex 
    {
        private string regex;
        private string regexDescribtion;
        private NFAModel defineNFAModel;

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
        public NFAModel DefineedNFAModel
        {
            get
            {
                return defineNFAModel;
            }
        }
        public Regex() { }
        public Regex(string regexDesc)
        {
            regexDescribtion = regexDesc;
        }

        //can only contain value
        //97-122,65-90,48-57
        //a-z，A-Z，1-9
        public static Regex DefineRange(char x, char y)
        {
            if(x > y)
            {
                var t = x;
                x = y;
                y = x;
            }

            Regex r = new Regex();
            var model = r.defineNFAModel = new NFAModel();
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
        public Regex DefineLiteral(string input)
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
            defineNFAModel = model;
            return this;
        }

        public static Regex CreateWithLiteral(string input)
        {
            Regex r = new Regex();
            r.DefineLiteral(input);
            return r;
        }
       

        //tested
        public Regex Union(Regex right)
        {
            var left = this;
            NFAModel model = new NFAModel();
            NFAState s = new NFAState(0);
            model.entryEdge = new NFAEdge(s, 0);

            s.AddEdge(left.defineNFAModel.entryEdge);
            s.AddEdge(right.defineNFAModel.entryEdge);

            model.tailState = new NFAState();
            left.defineNFAModel.tailState.AddEdgeTo(model.tailState);
            right.defineNFAModel.tailState.AddEdgeTo(model.tailState);

            defineNFAModel = model;
            return this;
        }

       

        //tested
        public Regex Concat(Regex right)
        {
            NFAModel model = this.defineNFAModel;
            model.tailState.AddEdge(right.defineNFAModel.entryEdge);
            model.tailState = right.defineNFAModel.tailState;
            return this;
        }

        public Regex KleeneStar()
        {
            NFAModel model = new NFAModel();
            model.tailState = new NFAState();
            model.entryEdge = new NFAEdge(model.tailState, 0);

            defineNFAModel.tailState.AddEdgeTo(model.tailState);
            model.tailState.AddEdge(defineNFAModel.entryEdge);

            defineNFAModel = model;
            return this;
        }

        public static Regex DefineKleeneStar(string input)
        {
            return CreateWithLiteral(input).KleeneStar();
        }

        public static Regex DefineConcat(Regex left, Regex right)
        {
            left.Concat(right);
            return left;
        }

        public static Regex DefineUnion(Regex left, Regex right)
        {
            left.Union(right);
            return left;
        }
    }
}
