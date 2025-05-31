using Domain.Lexer;

namespace Domain.Parser;

public class ParserContext
{
    public int Current;
    public bool IsAtEnd => Current >= Tokens.Count;
    public readonly List<Token> Tokens;

    public ParserContext(List<Token> tokens)
    {
        Tokens = tokens;
        Current = 0;
    }

    public void Expect(TokenType type, params HashSet<TokenSubType> expected)
    {
        if (IsAtEnd)
            throw new Exception("Unexpected end of input");

        var token = Pick();
        if (!expected.Contains(token.SubType) || token.Type != type)
            throw new Exception($"Expected token type {type} {expected}, but found {token.Type} {token.SubType} ({token.Lexeme})");
    }

    public Token Pick() => Tokens[Current];
    public void Advance() => Current++;
}
