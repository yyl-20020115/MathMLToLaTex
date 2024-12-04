using System.Text;

namespace MathMLToLaTeXTests
{
    [TestClass]
    public sealed class Test1
    {
        private readonly List<(string, string)> tests = [];

        [TestInitialize]
        public void Init()
        {
            using var reader = new StreamReader("tests.txt",Encoding.UTF8);
            string? line;
            var builder = new StringBuilder();

            var mathml = "";
            var latex = "";
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if(line == "----------------")
                {
                    mathml = builder.ToString();
                    builder.Clear();
                }else if(line == "================")
                {
                    latex = builder.ToString();
                    builder.Clear();
                    tests.Add((mathml, latex));
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
            foreach (var test in tests)
            {
                var result = MathMLToLaTex.MathMLToLaTeXConverter.Convert(test.Item1);
                Assert.AreEqual(result, test.Item2);    
            }
        }
    }
}
