using Domain.Lexer;

namespace App.Contracts.Lexer;

public interface ITokenHandler
{
    bool CanHandle(char current);
    Token? Handle(LexerContext context);
}
