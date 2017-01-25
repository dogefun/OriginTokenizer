using System;

namespace OriginTokenizer
{
    class  Program
    {
        static void AnotherTestCase()
        {
            ScannerInfo info = new ScannerInfo();
            RegularExpression regex1 = new RegularExpression();
            regex1.DefineLiteral("string");
        }
        static void Testcase()
        {
            ScannerInfo info = new ScannerInfo();

            RegularExpression regex1 = new RegularExpression();
            regex1.DefineLiteral("string");
            regex1.Describtion = "string key word";

            RegularExpression regex = new RegularExpression();
            regex.Describtion = "string with no number";
            regex.DefineLiteral("\"");
            regex.Concat(RegularExpression.DefineRange('a','z').Union(RegularExpression.DefineRange('A', 'Z')).KleeneStar()).Concat(RegularExpression.CreateWithLiteral("\""));

            RegularExpression regex2 = RegularExpression.CreateWithLiteral(" ");
            regex2.Describtion = "white space";

            info.AddRegex(regex2);
            info.AddRegex(regex1);
            info.AddRegex(regex);

            info.GenerateData();
            Scanner scanner = new Scanner(info);
            scanner.SetSkipTokenRegex(regex2);
            scanner.SetSource("\"adad\" string");
            var t = scanner.ReadAll();

            Console.WriteLine(/*t.Describtion*/t);

        }
        static void Main(string[] args)
        {
            Testcase();
            //Console.Read();
        }
    }
}
