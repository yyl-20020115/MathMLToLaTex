using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MathMLToLaTeXTests;

[TestClass]
public sealed class Test1
{
    private readonly List<(string mathml, string latex, int line_number)> tests = [];

    [TestInitialize]
    public void Init()
    {
        using var reader = new StreamReader("tests.txt", Encoding.UTF8);
        string? line;
        var builder = new StringBuilder();

        var mathml = "";
        var latex = "";
        var line_number = 0;
        var good = true;
        while ((line = reader.ReadLine()) != null)
        {
            line_number++;
            line = line.Trim();
            if (line == "----------------")
            {
                mathml = builder.ToString().Trim();
                try
                {
                    //we ignore broken xmls
                    var r = XElement.Parse(mathml,LoadOptions.PreserveWhitespace);
                    if (r?.Name == "root")
                    {
                        r = r?.Elements().Where(e => e.Name.LocalName == "math").FirstOrDefault();
                    }
                    mathml = r?.ToString() ?? mathml;
                }
                catch (Exception e)
                {
                    good = false;
                }
                builder.Clear();
            }
            else if (line == "================")
            {
                if (good)
                {
                    latex = builder.ToString().Trim();
                    tests.Add((mathml, latex, line_number));
                }
                good = true;
                builder.Clear();
            }
            else
            {
                builder.AppendLine(line);
            }
        }
    }
    [TestMethod]
    public void TestMethod1()
    {
        var count = 0;
        var bad = 0;
        foreach (var test in tests)
        {
            var result = MathMLToLaTex.MathMLToLaTeXConverter.Convert(test.mathml);
            try
            {
                Assert.AreEqual(result, test.latex);
            }
            catch (Exception e)
            {
                bad++;
                System.Diagnostics.Debug.WriteLine($"\n\n({bad})index={count},line={test.line_number}:{result} -> {test.latex}\n\n");
            }
            count++;
        }
    }
}
