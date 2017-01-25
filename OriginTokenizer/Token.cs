using System;

namespace OriginTokenizer
{
    public class Token :ICloneable
    {
        internal int status;
        public string value;
        public string Describtion;
        public int startPosition;
        /// <summary>
        /// Token.Index = RegularExpression.Index when using same s 
        /// </summary>
        public int Index
        {
            get
            {
                return status;
            }
        }
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

        public static Token ErrorToken
        {
            get
            {
                var x = new Token();
                x.status = -1;
                x.Describtion = "Cant understand token";
                return x;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
