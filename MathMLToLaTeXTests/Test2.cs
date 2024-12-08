namespace MathMLToLaTeXTests;

[TestClass]
public class Test2
{
    private readonly List<(string, string)> Tests =
    [
            (@"0",            @"<mn>0</mn>"),
            (@"3.14",         @"<mn>3.14</mn>"),
            (@"x",            @"<mi>x</mi>"),
            (@"\alpha",       @"<mi>α</mi>"),
            (@"\phi/\varphi", @"<mi>ϕ</mi><mo>/</mo><mi>φ</mi>"),
            (@"x = 3+\alpha", @"<mi>x</mi><mo>=</mo><mn>3</mn><mo>+</mo><mi>α</mi>"),
            (@"\sin x",       @"<mi>sin</mi><mi>x</mi>"),
            (@"\sqrt 2",      @"<msqrt><mn>2</mn></msqrt>"),
            (@"\sqrt12",      @"<msqrt><mn>1</mn></msqrt><mn>2</mn>"),
            (@"\sqrt{x+2}",   @"<msqrt><mrow><mi>x</mi><mo>+</mo><mn>2</mn></mrow></msqrt>"),
            (@"\sqrt[3]{x}",  @"<mroot><mi>x</mi><mn>3</mn></mroot>"),
            (@"\frac{1}{2}",  @"<mfrac><mn>1</mn><mn>2</mn></mfrac>"),
            (@"\frac12",      @"<mfrac><mn>1</mn><mn>2</mn></mfrac>"),
            (@"\frac{12}{5}", @"<mfrac><mn>12</mn><mn>5</mn></mfrac>"),
            (@"x^2",          @"<msup><mi>x</mi><mn>2</mn></msup>"),
            (@"g_{\mu\nu}",   @"<msub><mi>g</mi><mrow><mi>μ</mi><mi>ν</mi></mrow></msub>"),
            (@"\dot{x}",      "<mover><mi>x</mi><mo accent=\"true\">\\u{02d9}</mo></mover>"),
            (@"\sin x",       @"<mi>sin</mi><mi>x</mi>"),
            (@"\operatorname{sn} x", @"<mi>sn</mi><mi>x</mi>"),
            (@"\binom12",     "<mrow><mo stretchy=\"true\" form=\"prefix\">(</mo><mfrac linethickness=\"0\"><mn>1</mn><mn>2</mn></mfrac><mo stretchy=\"true\" form=\"postfix\">)</mo></mrow>"),
            (@"\left( x \right)", "<mrow><mo stretchy=\"true\" form=\"prefix\">(</mo><mi>x</mi><mo stretchy=\"true\" form=\"postfix\">)</mo></mrow>"),
            (@"\left( x \right.", "<mrow><mo stretchy=\"true\" form=\"prefix\">(</mo><mi>x</mi><mo stretchy=\"true\" form=\"postfix\"></mo></mrow>"),
            (@"\int dx",      @"<mo>∫</mo><mi>d</mi><mi>x</mi>"),
            (@"\oint_C dz",   @"<msub><mo>∮</mo><mi>C</mi></msub><mi>d</mi><mi>z</mi>"),
            (@"\overset{n}{X}", "<mover><mi>X</mi><mi>n</mi></mover>"),
            (@"\int_0^1 dx",  @"<msubsup><mo>∫</mo><mn>0</mn><mn>1</mn></msubsup><mi>d</mi><mi>x</mi>"),
            (@"\int^1_0 dx",  @"<msubsup><mo>∫</mo><mn>0</mn><mn>1</mn></msubsup><mi>d</mi><mi>x</mi>"),
            (@"\bm{x}",       "<mi mathvariant=\"bold-italic\">x</mi>"),
            (@"\mathbb{R}",   "<mi mathvariant=\"double-struck\">R</mi>"),
            (@"\sum_{i = 0}^∞ i", "<munderover><mo>∑</mo><mrow><mi>i</mi><mo>=</mo><mn>0</mn></mrow><mi mathvariant=\"normal\">∞</mi></munderover><mi>i</mi>"),
            (@"\prod_n n",        @"<munder><mo>∏</mo><mi>n</mi></munder><mi>n</mi>"),
            (@"x\ y",         "<mi>x</mi><mspace width=\"1em\"/><mi>y</mi>"),
            (
                @"\left\{ x  ( x + 2 ) \right\}",
                "<mrow><mo stretchy=\"true\" form=\"prefix\">{</mo><mrow><mi>x</mi><mo>(</mo><mi>x</mi><mo>+</mo><mn>2</mn><mo>)</mo></mrow><mo stretchy=\"true\" form=\"postfix\">}</mo></mrow>"
            ),
            (@"f'", @"<msup><mi>f</mi><mo>′</mo></msup>"),
            (
                @"\begin{pmatrix} x \\ y \end{pmatrix}",
                "<mrow><mo stretchy=\"true\" form=\"prefix\">(</mo><mtable><mtr><mtd><mi>x</mi></mtd></mtr><mtr><mtd><mi>y</mi></mtd></mtr></mtable><mo stretchy=\"true\" form=\"postfix\">)</mo></mrow>"
            )
    ];
    [TestMethod]
    public void TestMethod1()
    {
        var count = 0;
        var bad = 0;
        for (var i = 0; i < Tests.Count; i++)
        {
            (string latex, string mml) = Tests[i];

            var result = MaTeX.LaTexToMathML.LaTexToMathMLConverter.Convert(latex);
            try
            {
                Assert.AreEqual(result, mml);
            }
            catch (Exception e)
            {
                bad++;
                System.Diagnostics.Debug.WriteLine($"\n\n({bad})index={count}:{result} -> {mml}\n\n");
            }

            count++;
        }
    }
}
