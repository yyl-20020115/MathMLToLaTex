using System.Text;

namespace MaTeX.LaTexToMathML;

public class Lexer
{
    public readonly TextReader Reader;

    public Lexer(TextReader Reader)
    {
        this.Reader = Reader;
        this.ReadChar();
        this.ReadChar();
    }
    public void Close()
    {
        this.Reader.Close();
    }
    private char current = '\0';
    private char peek = '\0';
    public char Current => this.current;
    public char Peek => this.peek;

    public char ReadChar()
    {
        var c = this.current;
        this.current = this.peek;
        var n = this.Reader.Read();
        this.peek = n >= 0 ? (char)n : '\0';
        return c;
    }
    protected char SkipWhiteSpace()
    {
        while (this.current.IsSpace())
            this.ReadChar();
        return this.current;
    }

    protected Token ReadNumber()
    {
        var number = new StringBuilder();
        var has_period = false;
        while (this.current.IsAsciiDigit()
            || (this.current == '.' && !has_period))
        {
            if (this.current == '.') { has_period = true; }
            number.Append(this.ReadChar());
        }
        return new Token.Number(number.ToString());
    }
    protected Token? ReadCommand()
    {
        this.ReadChar();
        var first = this.ReadChar();
        if (first == '\0') return null;
        var command = new StringBuilder();
        command.Append(first);
        while (first.IsAsciiLetter() 
            && this.current.IsAsciiLetter())
            command.Append(this.ReadChar());

        return Token.FromCommand(command.ToString());
    }
    protected Token AfterReadChar(Token token)
    {
        this.ReadChar();
        return token;
    }
    public Token? NextToken()
        => this.SkipWhiteSpace() switch
        {
            '=' => new Token.Operator('='),
            ';' => new Token.Operator(';'),
            ',' => new Token.Operator(','),
            '.' => new Token.Operator('.'),
            '\'' => new Token.Operator('\''),
            '(' => new Token.Paren("("),
            ')' => new Token.Paren(")"),
            '{' => new Token.LBrace(),
            '}' => new Token.RBrace(),
            '[' => new Token.Paren("["),
            ']' => new Token.Paren("]"),
            '|' => new Token.Paren("|"),
            '+' => new Token.Operator('+'),
            '-' => new Token.Operator('-'),
            '*' => new Token.Operator('*'),
            '/' => new Token.Operator('/'),
            '!' => new Token.Operator('!'),
            '<' => new Token.Operator('<'),
            '>' => new Token.Operator('>'),
            '_' => new Token.Underscore(),
            '^' => new Token.Circumflex(),
            '&' => new Token.Ampersand(),
            '\0' => new Token.EOF(),
            ':' => this.peek == '=' ? new Token.Paren(this.ReadChar() + "=") : new Token.Operator(':'),
            '\\' => this.ReadCommand(),
            _ => null
        } is Token token
            ? AfterReadChar(token)
            : this.current.IsAsciiDigit()
                ? this.ReadNumber()
                : this.current.IsAsciiLetter()
                    ? new Token.Letter(this.current, Variant.Italic)
                    : new Token.Letter(this.current, Variant.Normal)
        ;
}
