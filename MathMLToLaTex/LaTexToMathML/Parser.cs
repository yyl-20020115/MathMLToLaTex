using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MaTeX.LaTexToMathML;

public class Parser
{
    public static Node SetVariant(Node node, Variant v) => node switch
    {
        Node.Letter letter => new Node.Letter(letter.c, v),
        Node.Row row => new Node.Row([.. row.ns.Select(n => SetVariant(n, v))]),
        _ => node
    };
    public Lexer Lexer;
    protected LatexError? e = null;
    protected Token current = new Token.Illegal('\0');
    protected Token peek = new Token.Illegal('\0');
    public Parser(Lexer lexer)
    {
        Lexer = lexer;
        lexer.NextToken();
        lexer.NextToken();
    }

    public (Node[] ns, LatexError? e) Parse()
    {
        List<Node> ns = [];
        while (this.current is not Token.EOF)
        {
            ns.Add(ParseNode());
            this.NextToken();
        }
        return ([.. ns], e);
    }

    public Token? NextToken()
    {
        this.current = this.peek with { };
        if (Token.ActOnDigit(current) && this.Lexer.Current.IsAsciiDigit())
        {
            var n = this.Lexer.Current;
            this.Lexer.ReadChar();
            return new Token.Number($"{n}");
        }
        else
        {
            return this.Lexer.NextToken();
        }
    }
    public string ParseText()
    {
        this.NextToken();
        var builder = new StringBuilder();
        while (this.current is Token.Letter letter)
        {
            builder.Append(letter.c);
            this.NextToken();
        }
        return builder.ToString();
    }
    public Node ParseNode()
    {
        var left = this.ParseSingleNode();
        switch (this.peek)
        {
            case Token.Underscore:
                {
                    this.NextToken();
                    this.NextToken();
                    var right = this.ParseNode();
                    return new Node.Subscript(left, right);
                }
            case Token.Circumflex:
                {
                    this.NextToken();
                    this.NextToken();
                    var right = this.ParseNode();
                    return new Node.Superscript(left, right);
                }
            default:
                return left;
        }
    }
    public (Node?, LatexError?) ParseGroup(Token endToken)
    {
        this.NextToken();
        List<Node> nodes = [];
        while (this.current != endToken)
        {
            if (this.current is Token.EOF)
            {
                return (null, LatexError.CreateUnexpectedTokenError(endToken with { }, this.current with { }));
            }
            nodes.Add(this.ParseNode());
            this.NextToken();
        }
        return nodes.Count == 1 ? (nodes[0], null) : (new Node.Row([.. nodes]), null);
    }

    public Node? ParseSingleNode()
    {
        Node? node = this.current switch
        {
            Token.Number n => new Node.Number(n.s),
            Token.Letter l => new Node.Letter(l.c, l.v),
            Token.Operator o => new Node.Operator(o.c),
            Token.NewLine n => null,

            _ => null
        };
        if(this.peek is Token.Operator ot && ot.c=='\'')
        {
            this.NextToken();
            return new Node.Superscript(node, new Node.Operator('\''));
        }
        return node;
    }

}
