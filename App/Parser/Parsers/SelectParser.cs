using App.Contracts.Parser;
using Domain.Lexer;
using Domain.Parser;
using Domain.Queries;

namespace App.Parser.Parsers;

public class SelectParser : IQueryParser
{
    private SelectQuery currentQuery = new();

    public bool CanParse(Token token)
        => token.Type == TokenType.Keyword && token.SubType == TokenSubType.Select;

    public IQuery Parse(ParserContext context)
    {
        currentQuery = new SelectQuery();

        ParseSelect(context);
        ParseFrom(context);
        
        if (context.IsAtEnd || context.Pick().SubType != TokenSubType.Where)
            return currentQuery;

        ParseWhere(context);

        return currentQuery;
    }

    private void ParseSelect(ParserContext context)
    {
        context.Advance();

        while (!context.IsAtEnd && context.Pick().SubType != TokenSubType.From)
        {
            context.Expect(TokenType.Identifier, TokenSubType.Column, TokenSubType.Operator);
            currentQuery.Columns.Add(context.Pick().Lexeme);
            context.Advance();
        }
        context.Expect(TokenType.Keyword, TokenSubType.From);
    }

    private void ParseFrom(ParserContext context)
    {
        context.Advance();
        context.Expect(TokenType.Identifier, TokenSubType.Table);
        currentQuery.Table = context.Pick().Lexeme;
        context.Advance();
    }

    private void ParseWhere(ParserContext context)
    {
        context.Advance();
        context.Expect(TokenType.Identifier, TokenSubType.Column, TokenSubType.String, TokenSubType.Number);
        var column = context.Pick().Lexeme;
        context.Advance();
        context.Expect(TokenType.Identifier, TokenSubType.Operator);
        var op = context.Pick().Lexeme;
        context.Advance();
        context.Expect(TokenType.Identifier, TokenSubType.Column, TokenSubType.String, TokenSubType.Number);
        var value = context.Pick().Lexeme;
        currentQuery.Condition = new Condition { Column = column, Operator = op, Value = value };
        context.Advance();
    }
}
