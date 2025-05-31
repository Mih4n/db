using App.Contracts.Lexer;
using Domain.Lexer;

namespace App.Lexer.TokenHandlers;

public class OperatorHandler : ITokenHandler
{
    public bool CanHandle(char current)
        => IsOperator(current);

    public Token? Handle(LexerContext context)
    {
        var symbol = context.ReadWhile(IsOperator);
        return new Token(TokenType.Identifier, TokenSubType.Operator, symbol);
    }

    private HashSet<char> Operators = [
        '=', '>', '<', '!', '&', '|', '+', '-', '*', '/', '%'
    ];

    private bool IsOperator(char c) => Operators.Contains(c);
}
