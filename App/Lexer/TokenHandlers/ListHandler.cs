using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.Lexer;
using Domain.Lexer;

namespace App.Lexer.TokenHandlers;

public class ListHandler : ITokenHandler
{
    public bool CanHandle(char current)
        => current == '(' || current == ')';

    public Token? Handle(LexerContext context)
    {
        if (context.Peek() == '(')
        {
            context.Advance();
            return new Token(TokenType.Punctuation, TokenSubType.OpenParenthesis, "(");
        }
        else if (context.Peek() == ')')
        {
            context.Advance();
            return new Token(TokenType.Punctuation, TokenSubType.CloseParenthesis, ")");
        }
        return null;
    }
}
