using System.Text;

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
        while (this.current is not Token.EOF and not Token.Illegal)
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
    public (Node? node, LatexError? error) ParseGroup(Token endToken)
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
        Node? node = null;
        switch (this.current)
        {
            case Token.Number number:
                {
                    node = new Node.Number(number.s);
                }
                break;
            case Token.Letter letter:
                {
                    node = new Node.Letter(letter.c, letter.v);
                }
                break;
            case Token.Operator @operator:
                {
                    node = new Node.Operator(@operator.c);
                }
                break;
            case Token.Function function:
                {
                    node = new Node.Function(function.s, null);
                }
                break;
            case Token.Space space:
                {
                    node = new Node.Space(space.f);
                }
                break;
            case Token.Sqrt:
                {
                    this.NextToken();
                    Node? degree = null;
                    if (this.current is Token.Paren p && p.s == "[")
                    {
                        degree = this.ParseGroup(new Token.Paren("]")).node;
                        this.NextToken();
                    }
                    var content = this.ParseNode();
                    node = new Node.Sqrt(degree, content);
                }
                break;
            case Token.Frac:
                {
                    this.NextToken();
                    var numerator = this.ParseNode();
                    this.NextToken();
                    var denominator = this.ParseNode();
                    node = new Node.Frac(numerator, denominator, LineThickness.Medium);
                }
                break;
            case Token.Binom binom:
                {
                    this.NextToken();
                    var numerator = this.ParseNode();
                    this.NextToken();
                    var denominator = this.ParseNode();
                    var _binom = new Node.Fenced("(", ")", new Node.Frac(numerator, denominator, LineThickness.Length, 0));
                    node = binom.s != null ? new Node.Style(binom.s.Value, new Node.Row([_binom])) : _binom;
                }
                break;
            case Token.Over over:
                {
                    this.NextToken();
                    node = new Node.OverOp(over.c, over.Accent, this.ParseNode());
                }
                break;
            case Token.Under under:
                {
                    this.NextToken();
                    node = new Node.UnderOp(under.c, under.Accent, this.ParseNode());
                }
                break;
            case Token.Overset:
                {
                    this.NextToken();
                    var over = this.ParseNode();
                    this.NextToken();
                    var target = this.ParseNode();
                    node = new Node.Overset(over, target);
                }
                break;
            case Token.Underset:
                {
                    this.NextToken();
                    var under = this.ParseNode();
                    this.NextToken();
                    var target = this.ParseNode();
                    node = new Node.Underset(under, target);
                }
                break;
            case Token.Overbrace overbrace:
                {
                    this.NextToken();
                    var target = this.ParseSingleNode();
                    if (this.peek is Token.Circumflex)
                    {
                        this.NextToken();
                        this.NextToken();
                        var exp1 = this.ParseSingleNode();
                        var over = new Node.Overset(exp1, new Node.Operator(overbrace.c));
                        node = new Node.Overset(over, target);
                    }
                    else
                    {
                        node = new Node.Overset(new Node.Operator(overbrace.c), target);
                    }
                }
                break;
            case Token.Underbrace underbrace:
                {
                    this.NextToken();
                    var target = this.ParseSingleNode();
                    if (this.peek is Token.Circumflex)
                    {
                        this.NextToken();
                        this.NextToken();
                        var exp1 = this.ParseSingleNode();
                        var over = new Node.Overset(exp1, new Node.Operator(underbrace.c));
                        node = new Node.Overset(over, target);
                    }
                    else
                    {
                        node = new Node.Overset(new Node.Operator(underbrace.c), target);
                    }
                }
                break;
            case Token.BigOp bigOp:
                {
                    switch (this.peek)
                    {
                        case Token.Underscore:
                            {
                                this.NextToken();
                                this.NextToken();
                                var under = this.ParseSingleNode();
                                if (this.peek is Token.Circumflex)
                                {
                                    this.NextToken();
                                    this.NextToken();
                                    var over = this.ParseSingleNode();

                                    node = new Node.UnderOver(new Node.Operator(bigOp.c), under, over);
                                }
                                else
                                {
                                    node = new Node.Under(new Node.Operator(bigOp.c), under);
                                }
                            }
                            break;
                        case Token.Circumflex:
                            {
                                this.NextToken();
                                this.NextToken();
                                var over = this.ParseSingleNode();
                                if (this.peek is Token.Underscore)
                                {
                                    this.NextToken();
                                    this.NextToken();
                                    var under = this.ParseSingleNode();

                                    node = new Node.UnderOver(new Node.Operator(bigOp.c), under, over);
                                }
                                else
                                {
                                    node = new Node.OverOp(bigOp.c, false, over);
                                }
                            }
                            break;
                        default:
                            node = new Node.Operator(bigOp.c);
                            break;
                    }
                }
                break;
            case Token.Lim lim:
                {
                    var _lim = new Node.Function($"{lim.s}", null);
                    if (this.peek is Token.Underscore)
                    {
                        this.NextToken();
                        this.NextToken();
                        var under = this.ParseSingleNode();
                        node = new Node.Under(_lim, under);
                    }
                    else
                    {
                        node = _lim;
                    }
                }
                break;
            case Token.Slashed:
                {
                    this.NextToken();
                    this.NextToken();
                    var n = this.ParseNode();
                    this.NextToken();
                    node = new Node.Slashed(n);
                }
                break;
            case Token.Style style:
                {
                    this.NextToken();
                    node = this.ParseNode();
                    SetVariant(node, style.v);
                }
                break;
            case Token.Integral integral:
                {
                    switch (this.peek)
                    {
                        case Token.Underscore:
                            {
                                this.NextToken();
                                this.NextToken();
                                var sub = this.ParseSingleNode();
                                if (this.peek is Token.Circumflex)
                                {
                                    this.NextToken();
                                    this.NextToken();
                                    var sup = this.ParseSingleNode();

                                    node = new Node.SubSup(new Node.Operator(integral.c), sub, sup);
                                }
                                else
                                {
                                    node = new Node.Subscript(new Node.Operator(integral.c), sub);
                                }
                            }
                            break;
                        case Token.Circumflex:
                            {
                                this.NextToken();
                                this.NextToken();
                                var sup = this.ParseSingleNode();
                                if (this.peek is Token.Underscore)
                                {
                                    this.NextToken();
                                    this.NextToken();
                                    var sub = this.ParseSingleNode();

                                    node = new Node.SubSup(new Node.Operator(integral.c), sub, sup);
                                }
                                else
                                {
                                    node = new Node.Superscript(new Node.Operator(integral.c), sup);
                                }
                            }
                            break;
                        default:
                            node = new Node.Operator(integral.c);
                            break;
                    }
                }
                break;
            case Token.LBrace:
                {
                    node = this.ParseGroup(new Token.RBrace()).node;
                }
                break;
            case Token.Paren paren:
                {
                    node = new Node.OtherOperator(paren.s);
                }
                break;
            case Token.Left:
                {
                    this.NextToken();
                    var open = "";
                    switch (this.current)
                    {
                        case Token.Paren p:
                            open = p.s;
                            break;
                        case Token.Operator to:
                            if (to.c == '.')
                            {
                                open = "";
                            }
                            else
                            {
                                this.e = LatexError.CreateMissingParensethisError(new Token.Left(), current with { });
                            }
                            break;
                    };

                    var content = this.ParseGroup(new Token.Right()).node;
                    this.NextToken();
                    var close = "";
                    switch (this.current)
                    {
                        case Token.Paren p:
                            open = p.s;
                            break;
                        case Token.Operator to:
                            if (to.c == '.')
                            {
                                open = "";
                            }
                            else
                            {
                                this.e = LatexError.CreateMissingParensethisError(new Token.Right(), current with { });
                            }
                            break;
                    };

                    node = new Node.Fenced(open, close, content);
                }
                break;
            case Token.Middle:
                {
                    var stretchy = true;
                    this.NextToken();
                    var n = this.ParseSingleNode();
                    node = n switch
                    {
                        Node.Operator no => new Node.StrechedOp(stretchy, $"{no.c}"),
                        Node.OtherOperator oo => new Node.StrechedOp(stretchy, $"{oo.s}"),
                        _ => null,
                    };
                }
                break;
            case Token.Big big:
                {
                    this.NextToken();
                    switch (this.current)
                    {
                        case Token.Paren p:
                            node = new Node.SizedParen(big.s, p.s);
                            break;
                        default:
                            node = null;
                            this.e = LatexError.CreateUnexpectedTokenError(new Token.Paren(""), this.current with { });
                            break;
                    }
                }
                break;
            case Token.Begin:
                {
                    this.NextToken();
                    var environment = this.ParseText() ?? "";
                    var alignment = ColumnAlign.Center;
                    if (environment == "align")
                    {
                        alignment = ColumnAlign.Left;
                        environment = "matrix";
                    }
                    Node[] ns = [];
                    var g = this.ParseGroup(new Token.End());
                    if (g.node is Node.Row row)
                    {
                        ns = row.ns;
                    }
                    else if (g.node != null)
                    {
                        ns = [g.node];
                    }
                    var content = new Node.Matrix(ns, alignment);

                    Node? matrix = environment switch
                    {
                        "matrix" => content,
                        "pmatrix" => new Node.Fenced("(", ")", content),
                        "bmatrix" => new Node.Fenced("[", "]", content),
                        "vmatrix" => new Node.Fenced("|", "|", content),
                        _ => null
                    };
                    if (matrix == null)
                    {
                        this.e = LatexError.CreateUnknownEnvironmentError(environment);
                    }
                    this.NextToken();
                    this.ParseText();

                    node = matrix;
                }
                break;
            case Token.OperatorName:
                {
                    this.NextToken();
                    node = new Node.Function(this.ParseText(), null);
                }
                break;
            case Token.Text:
                {
                    this.NextToken();
                    node = new Node.Text(this.ParseText());
                }
                break;
            case Token.Ampersand:
                {
                    node = new Node.Ampersand();
                }
                break;
            case Token.NewLine:
                {
                    node = new Node.NewLine();
                }
                break;
            default:
                {
                    node = new Node.Undefined($"{this.current}");
                }
                break;
        };
        if (this.peek is Token.Operator _ot && _ot.c == '\'')
        {
            this.NextToken();
            return new Node.Superscript(node, new Node.Operator('\''));
        }
        return node;
    }
}
