using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.Lexer;
using Domain.Lexer;

namespace App.Lexer.TokenHandlers;

public class StringHandler : ITokenHandler
{
    public bool CanHandle(char current)
        => IsQuote(current);

    public Token? Handle(LexerContext context)
    {
        context.Advance();
        var lexeme = context.ReadWhile(c => !IsQuote(c));
        if (!context.IsAtEnd && IsQuote(context.Peek()))
            context.Advance();
        return new Token(TokenType.Identifier, TokenSubType.String, lexeme);
    }

    private bool IsQuote(char c) => c == '"' || c == '\'';
}
