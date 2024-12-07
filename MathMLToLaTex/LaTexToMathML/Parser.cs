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
    protected Token current = new Token.Illegal('\0');
    protected Token next = new Token.Illegal('\0');
    public Parser(Lexer lexer)
    {
        Lexer = lexer;
        lexer.NextToken();
        lexer.NextToken();
    }



}
