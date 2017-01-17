using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OriginTokenizer.Utils;
namespace OriginTokenizer
{
    class  Program
    {
        static void Testcase()
        {
            Regex r = new Regex();
            r = Regex.CreateWithLiteral("a").Union(Regex.CreateWithLiteral("b")).KleeneStar().Concat(Regex.CreateWithLiteral("abb"));
            //r.Concat
            r.DefineedNFAModel.RenameStates(1);

            //Console.WriteLine(r.DefineedNFAModel.ShowStatesS());
            DFAModel m = new DFAModel();
            m.SetRegularExpression(r);
            m.CreateDFAModel();
            //DFAState s = new DFAState(r.DefineedNFAModel.entryEdge.lead);
            //DFAModel m = DFAModel.CreateDFAModel(r.DefineedNFAModel);

            Console.WriteLine(m.Properites());
        }
        static void Main(string[] args)
        {
            Testcase();
            Console.Read();
        }
    }
}
