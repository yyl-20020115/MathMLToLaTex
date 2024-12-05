namespace MaTeX.LaTexToMathML;

public abstract class LatexError
{
    public static LatexError CreateUnexpectedTokenError(Token Expected, Token Got)
        => new UnexpectedTokenError(Expected, Got);

    public static LatexError CreateMissingParensethisError(Token Location, Token Got)
        => new MissingParensethisError(Location, Got);

    public static LatexError CreateUnknownEnvironmentError(string s)
        => new UnknownEnvironmentError(s);

    public static LatexError CreateInvalidNumberOfDollarSignsError()
        => new InvalidNumberOfDollarSignsError();

    public class UnexpectedTokenError(Token Expected, Token Got) : LatexError
    {
        public readonly Token Expected = Expected;
        public readonly Token Got = Got;
    };
    public class MissingParensethisError(Token Location, Token Got) : LatexError
    {
        public readonly Token Location = Location;
        public readonly Token Got = Got;
    };
    public class UnknownEnvironmentError(string s) : LatexError
    {
        public readonly string s = s;
    }
    public class InvalidNumberOfDollarSignsError : LatexError { }

    public static string ToString(LatexError error, string s, Token expected, Token location, Token got) => error switch
    {
        UnexpectedTokenError => $"The token \"{expected}\" is expected, but the token \"{got}\" is found.\"",
        MissingParensethisError => $"There must be a parenthesis after \"{location}\", but not found. Insted, \"{got}\" is found.",
        UnknownEnvironmentError => $"An unknown environment \"{s}\" is found",
        InvalidNumberOfDollarSignsError => "The number of dollar sings found is invalid.",
        _ => ""
    };
}
