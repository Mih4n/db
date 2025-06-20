using Domain.Lexer;
using Domain.Parser;

namespace App.Parser;

public class Parser
{
    public Expression Parse(List<Token> tokens)
    {
        var context = new ParserContext(tokens);
        return ParseExpression(context, 0);
    }

    private Expression ParseExpression(ParserContext context, float minBp)
    {
        Expression lhs;
        Expression rhs;
        
        Token token = context.Pick();
        context.Advance();

        if (token.Type == TokenType.Literal || token.Type == TokenType.Identifier)
        {
            lhs = token;
        }
        else if (token.Type == TokenType.Keyword)
        {
            lhs = ParseExpression(context, token.GetBindingPower().rightBp);
            lhs = new Operation(token, [lhs]);
        }
        else if (token.Type == TokenType.Punctuation && token.SubType == TokenSubType.OpenParenthesis)
        {

            lhs = ParseExpression(context, 0);

            context.Expect(TokenType.Punctuation, TokenSubType.CloseParenthesis);

            return lhs;
        }
        else
        {
            throw new Exception($"Bad token: {token}");
        }

        while (true)
        {
            if (context.IsAtEnd) break;

            Token lookahead = context.Pick();
            if (lookahead.SubType == TokenSubType.CloseParenthesis) break;

            context.Expect(TokenType.Operator | TokenType.Keyword, TokenSubType.Any);

            (float leftBp, float rightBp) = lookahead.GetBindingPower();

            if (leftBp <= minBp) break;

            context.Advance();

            rhs = ParseExpression(context, rightBp);

            lhs = new Operation(lookahead, [lhs, rhs]);
        }

        return lhs;
    }
}
