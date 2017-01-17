using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginTokenizer.Utils
{
    class Regex 
    {
        private string regex;
        private string regexDescribtion;
        private NFAModel defineNFAModel;

        public NFAModel DefineedNFAModel
        {
            get
            {
                return defineNFAModel;
            }
        }
        
        private void ShortenNFA(NFAEdge edge)
        {
            //var entry = defineNFAModel.entryEdge;
            var model = edge.lead;
            if (model.Lead.Count == 0)
            {
                //model.isEndState = true;//争议
                return;
            }
            else if(model.Lead.Count == 1)
            {
                if(model.Lead[0].statement == 0)
                {
                    edge.lead = model.Lead[0].lead;
                }
            }



        }

        //需要加更严格的规范
        //a-z，A-Z，1-9
        public static Regex DefineRange(string x,string y)
        {
            if(x.Length > 1 || y.Length > 1 || x[0] > y[0])
            {
                throw new Exception("Input Error");
            }

            Regex r = new Regex();
            var model = r.defineNFAModel = new NFAModel();
            NFAState s = new NFAState(0);
            model.tailState = new NFAState(1);
            model.entryEdge = new NFAEdge(s);

            for(int i = x[0];i <= y[0]; i++)
            {
                s.AddEdge(new NFAEdge(model.tailState, i));
            }

            
            return r;

        }

        //basisOp
        //clean unnessary edge

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

            //create a new tail
            //is it need?
            //model.tailState = new NFAState();
            //right.defineNFAModel.tailState.AddEdgeTo(model.tailState);

            //test modify
            model.tailState = right.defineNFAModel.tailState;
            //defineNFAModel = model;
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

        //tested
        public static Regex DefineConcat(Regex left, Regex right)
        {
            left.Concat(right);
            return left;
        }

        //tested
        public static Regex DefineUnion(Regex left, Regex right)
        {
            left.Union(right);
            return left;
        }
    }
}
