using System.Text;

namespace MathMLToLaTex;

internal class Program
{

    static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            Console.WriteLine(MathMLToLaTeX.Convert(args[0]));
        }
    }
}
