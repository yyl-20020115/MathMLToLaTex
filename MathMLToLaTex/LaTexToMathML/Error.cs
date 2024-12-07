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

    public class UnexpectedTokenError(Token expected, Token got) : LatexError
    {
        public readonly Token expected = expected;
        public readonly Token got = got;
        public override string ToString()
            => $"The token \"{expected}\" is expected, but the token \"{got}\" is found.\"";
    };
    public class MissingParensethisError(Token location, Token got) : LatexError
    {
        public readonly Token location = location;
        public readonly Token got = got;
        public override string ToString()
            => $"There must be a parenthesis after \"{location}\", but not found. Insted, \"{got}\" is found.";
    };
    public class UnknownEnvironmentError(string s) : LatexError
    {
        public readonly string s = s;
        public override string ToString()
            => $"An unknown environment \"{s}\" is found";
    }
    public class InvalidNumberOfDollarSignsError : LatexError 
    {
        public override string ToString()
            => "The number of dollar sings found is invalid.";
    }
}
