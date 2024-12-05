namespace MaTex.MathMLToLaTex;

public static class MathMLElementToLatexConverterAdapter
{
    public static readonly Dictionary<string, Type> ConverterTypes = new()
    {
        ["math"] = typeof(ToLatexConverters.Math),
        ["mi"] = typeof(ToLatexConverters.MI),
        ["mo"] = typeof(ToLatexConverters.MO),
        ["mn"] = typeof(ToLatexConverters.MN),
        ["msqrt"] = typeof(ToLatexConverters.MSqrt),
        ["mfenced"] = typeof(ToLatexConverters.MFenced),
        ["mfrac"] = typeof(ToLatexConverters.MFrac),
        ["mroot"] = typeof(ToLatexConverters.MRoot),
        ["maction"] = typeof(ToLatexConverters.MAction),
        ["menclose"] = typeof(ToLatexConverters.MEnclose),
        ["merror"] = typeof(ToLatexConverters.MError),
        ["mphantom"] = typeof(ToLatexConverters.MPhantom),
        ["msup"] = typeof(ToLatexConverters.MSup),
        ["msub"] = typeof(ToLatexConverters.MSub),
        ["msubsup"] = typeof(ToLatexConverters.MSubsup),
        ["mmultiscripts"] = typeof(ToLatexConverters.MMultiscripts),
        ["mtext"] = typeof(ToLatexConverters.MText),
        ["munderover"] = typeof(ToLatexConverters.MUnderover),
        ["mtable"] = typeof(ToLatexConverters.MTable),
        ["mtr"] = typeof(ToLatexConverters.MTr),
        ["mover"] = typeof(ToLatexConverters.GenericUnderOver),
        ["munder"] = typeof(ToLatexConverters.GenericUnderOver),
        ["mrow"] = typeof(ToLatexConverters.GenericSpacingWrapper),
        ["mpadded"] = typeof(ToLatexConverters.GenericSpacingWrapper),
        ["void"] = typeof(ToLatexConverters.Void),

    };
    public static readonly ToLatexConverter DefaultConverter = new ToLatexConverters.Void(MathMLElement.Default);
    public static ToLatexConverter Convert(MathMLElement mathMLElement) 
        =>(ConverterTypes.TryGetValue(mathMLElement.Name.ToLower(), out var type)
            ? type : typeof(ToLatexConverters.GenericSpacingWrapper))?.GetConstructor([typeof(MathMLElement)])?.
                Invoke([mathMLElement]) as ToLatexConverter ?? DefaultConverter;
}
