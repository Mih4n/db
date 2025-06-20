namespace Domain.Lexer;

[Flags]
public enum TokenType
{
    Literal = 1,
    Keyword = 2,
    Operator = 4,
    Identifier = 8,
    Punctuation = 16,
}
