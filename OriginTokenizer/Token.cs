namespace OriginTokenizer
{
    public class Token
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
    }
}
