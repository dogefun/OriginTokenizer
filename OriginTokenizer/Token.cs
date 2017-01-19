using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginTokenizer
{
    class Token
    {
        public int status;
        public string value;
        public string Describtion;

        public static Token EndOfSourceToken
        {
            get
            {
                var x = new Token();
                x.status = -1;
                x.Describtion = "End of Text and Read Nothing";
                return x;
            }
        }
    }
}
