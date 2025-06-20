using App.Contracts.Lexer;
using Domain.Lexer;

namespace App.Lexer.TokenHandlers;

public class OperatorHandler : ITokenHandler
{
    private Dictionary<string, TokenSubType> OperatorSubTypes = new()
    {
        { "=", TokenSubType.Assignment },
        { ">", TokenSubType.GreaterThan },
        { "<", TokenSubType.LessThan },
        { "!=", TokenSubType.NotEqual },
        { "==", TokenSubType.Equal },
        { "&&", TokenSubType.And },
        { "||", TokenSubType.Or },
        { "+", TokenSubType.Addition },
        { "-", TokenSubType.Subtraction },
        { "*", TokenSubType.Multiplication },
        { "/", TokenSubType.Division },
        { "%", TokenSubType.Modulus }
    };

    public bool CanHandle(char current)
        => IsOperator(current);

    public Token? Handle(LexerContext context)
    {
        var symbol = context.ReadWhile(IsOperator);
        if (!OperatorSubTypes.TryGetValue(symbol, out var subType)) return null;
        return new Token(TokenType.Operator, subType, symbol);
    }

    private HashSet<char> Operators = [
        '=', '>', '<', '!', '&', '|', '+', '-', '*', '/', '%'
    ];

    private bool IsOperator(char c) => Operators.Contains(c);
}
