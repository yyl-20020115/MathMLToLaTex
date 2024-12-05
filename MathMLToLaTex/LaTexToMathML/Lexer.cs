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

    public char Current { get; private set; } = '\0';
    public char Peek { get; private set; } = '\0';

    protected char ReadChar()
    {
        var c = Current;
        this.Current = this.Peek;
        var n = this.Reader.Read();
        this.Peek = n >= 0 ? (char)n : '\0';
        return c;
    }
    protected char SkipWhiteSpace()
    {
        while (this.Current == ' ' || this.Current == '\t' || this.Current == '\n' || this.Current == '\r')
        {
            this.ReadChar();
        }
        return this.Current;
    }

    protected Token ReadNumber()
    {
        var number = new StringBuilder();
        var has_period = false;
        while (this.Current >= 0 && this.Current <= 0xff && char.IsDigit(this.Current)
            || (this.Current == '.' && !has_period))
        {
            if (this.Current == '.') { has_period = true; }
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
        while (first >= 0 && first <= 0xff && char.IsLetter(first)
            && this.Current >= 0 && first <= this.Current && char.IsLetter(this.Current))
            command.Append(this.ReadChar());

        return Token.FromCommand(command.ToString());
    }
    public Token? NextToken()
    {
        var token = this.SkipWhiteSpace() switch
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
            ':' => this.Peek == '=' ? new Token.Paren(this.ReadChar() + "=") : new Token.Operator(':'),
            '\\' => this.ReadCommand(),
            _ => null
        };
        if (token != null)
        {
            this.ReadChar();
            return token;
        }
        else
        {
            return this.Current >= 0 && this.Current <= 0xff && char.IsDigit(this.Current)
                ? this.ReadNumber()
                : this.Current >= 0 && this.Current <= 0xff && char.IsLetter(this.Current)
                    ? new Token.Letter(this.Current, Variant.Italic)
                    : new Token.Letter(this.Current, Variant.Normal);
        };
    }
}
