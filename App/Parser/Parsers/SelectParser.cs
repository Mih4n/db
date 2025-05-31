using App.Contracts.Parser;
using Domain.Lexer;
using Domain.Parser;
using Domain.Queries;

namespace App.Parser.Parsers;

public class SelectParser : IQueryParser
{
    private readonly SelectQuery currentQuery = new();

    public bool CanParse(Token token)
        => token.Type == TokenType.keyword && token.SubType == TokenSubType.Select;

    public IQuery Parse(ParserContext context)
    {
        ParseSelect(context);
        ParseFrom(context);
        ParseWhere(context);
        return currentQuery;
    }

    private void ParseSelect(ParserContext context)
    {
        context.Advance();

        while (!context.IsAtEnd && context.Pick().SubType != TokenSubType.From)
        {
            context.Expect(TokenType.Identifier, TokenSubType.Word, TokenSubType.Operator);
            currentQuery.Columns.Add(context.Pick().Lexeme);
            context.Advance();
        }
        context.Expect(TokenType.keyword, TokenSubType.From);
    }

    private void ParseFrom(ParserContext context)
    {
        context.Advance();
        context.Expect(TokenType.Identifier, TokenSubType.Word);
        currentQuery.Table = context.Pick().Lexeme;
        context.Advance();
    }

    private void ParseWhere(ParserContext context)
    {
        context.Advance();
        context.Expect(TokenType.Identifier, TokenSubType.Word, TokenSubType.String, TokenSubType.Number);
        var column = context.Pick().Lexeme;
        context.Advance();
        context.Expect(TokenType.Identifier, TokenSubType.Operator);
        var op = context.Pick().Lexeme;
        context.Advance();
        context.Expect(TokenType.Identifier, TokenSubType.Word, TokenSubType.String, TokenSubType.Number);
        var value = context.Pick().Lexeme;
        currentQuery.Condition = new Condition { Column = column, Operator = op, Value = value };
        context.Advance();
    }
}
