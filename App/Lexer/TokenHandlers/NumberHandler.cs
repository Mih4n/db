using App.Contracts.Lexer;
using Domain.Lexer;

namespace App.Lexer.TokenHandlers;

public class NumberHandler : ITokenHandler
{
    public bool CanHandle(char current)
        => IsNumber(current);

    public Token? Handle(LexerContext context)
    {
        var number = context.ReadWhile(IsNumber);
        return new Token(TokenType.Identifier, TokenSubType.Number, number);
    }

    private static bool IsNumber(char c)
        => char.IsDigit(c) || c == '.' || c == 'e' || c == 'E' || c == '+' || c == '-';
}
