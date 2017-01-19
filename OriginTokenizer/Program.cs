using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginTokenizer
{
    class  Program
    {
        static void Testcase()
        {
            ScannerInfo info = new ScannerInfo();

            Regex regex1 = new Regex();
            regex1.DefineLiteral("string");
            regex1.Describtion = "string key word";

            Regex regex = new Regex();
            regex.Describtion = "string with no number";
            regex.DefineLiteral("\"");
            regex.Concat(Regex.DefineRange('a','z').Union(Regex.DefineRange('A', 'Z')).KleeneStar()).Concat(Regex.CreateWithLiteral("\""));

            info.AddRegex(regex1);
            info.AddRegex(regex);

            info.CreateInfo();//try to remove this
            Scanner scanner = new Scanner(info);

            scanner.SetSource("\"adad\"string");
            var t = scanner.Read();
            t = scanner.Read();
            Console.WriteLine(t.value);

        }
        static void Main(string[] args)
        {
            Testcase();
            Console.Read();
        }
    }
}
