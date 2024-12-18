﻿using MaTex.MathMLToLaTex.Helpers.Wrappers;

namespace MaTex.MathMLToLaTex.ToLatexConverters;

public class MScript(MathMLElement Element) : ToLatexConverter(Element)
{
    protected static string ConvertBase(MathMLElement element)
    {
        var text = MathMLElementToLatexConverterAdapter.Convert(element).Convert();
        return element.Children.Length <= 1 ? text : new ParenthesisWrapper().WrapIfMore(text);
    }
    protected static string ConvertScript(MathMLElement element)
        => new BracketWrapper().Wrap(MathMLElementToLatexConverterAdapter.Convert(element).Convert());
    protected static string ConvertScriptNoBracket(MathMLElement element)
        => MathMLElementToLatexConverterAdapter.Convert(element).Convert();

}

