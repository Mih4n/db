using App.Contracts.Parser;
using Domain.Lexer;
using Domain.Parser;
using Domain.Queries;

namespace App.Parser.Parsers;

public class CreateParser : IQueryParser
{
    private CreateQuery currentQuery = new();

    public bool CanParse(Token token)
        => token.Type == TokenType.Keyword && token.SubType == TokenSubType.Create;

    public IQuery Parse(ParserContext context)
    {
        currentQuery = new CreateQuery();

        context.Expect(TokenType.Keyword, TokenSubType.Create);
        context.Advance();
        context.Expect(TokenType.Keyword, TokenSubType.Table);
        context.Advance();

        context.Expect(TokenType.Identifier, TokenSubType.Table);
        var tableName = context.Pick().Lexeme;
        context.Advance();

        currentQuery.TableName = tableName;
        currentQuery.Columns = ParseColumns(context);

        return currentQuery;
    }

    private List<ColumnDefinition> ParseColumns(ParserContext context)
    {
        var columns = new List<ColumnDefinition>();

        context.Expect(TokenType.Punctuation, TokenSubType.OpenParenthesis);
        context.Advance();

        while (!context.IsAtEnd && context.Pick().SubType != TokenSubType.CloseParenthesis)
        {
            context.Expect(TokenType.Identifier, TokenSubType.Column);

            var columnName = context.Pick().Lexeme;
            columns.Add(new ColumnDefinition { Name = columnName, Type = "string" });

            context.Advance();
        }

        context.Expect(TokenType.Punctuation, TokenSubType.CloseParenthesis);
        context.Advance();

        return columns;
    }
}
