﻿using MaTex.MathMLToLaTex.Helpers;

namespace MaTex.MathMLToLaTex.ToLatexConverters;

public class GenericUnderOver(MathMLElement Element) : ToLatexConverter(Element)
{
    protected static string GetDefaultCmd(bool under) => under ? "\\underset" : "\\overset";
    public override string Convert()
    {
        if (this.Element.Children.Length != 2) throw new InvalidDataException(nameof(Element.Children));

        var content = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[0]).Convert();
        var accent = MathMLElementToLatexConverterAdapter.Convert(this.Element.Children[1]).Convert();
        var cmd = GetDefaultCmd(this.Element.Name.Equals("munder",StringComparison.OrdinalIgnoreCase));
        
        return (Dicts.LatexAccents.Contains(accent)) ?
            $"{accent}{{{content}}}" : $"{cmd}{{{accent}}}{{{content}}}";
    }
}
