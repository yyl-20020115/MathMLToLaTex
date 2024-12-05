using MaTex.MathMLToLaTex;

namespace MaTeX.LaTexToMathML;

public static class LaTexToMathMLConverter
{
    public static string Convert(string latex)
    {
        MathMLElement mathML = new();

        return MathMLToElementsAdapter.Convert(mathML).ToString();
    }
}
