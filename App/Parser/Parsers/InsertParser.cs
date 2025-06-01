using App.Contracts.Parser;
using Domain.Lexer;
using Domain.Parser;
using Domain.Queries;

namespace App.Parser.Parsers;

public class InsertParser : IQueryParser
{
    private InsertQuery currentQuery = new();

    public bool CanParse(Token token)
        => token.Type == TokenType.Keyword && token.SubType == TokenSubType.Insert;

    public IQuery Parse(ParserContext context)
    {
        context.Expect(TokenType.Keyword, TokenSubType.Insert);
        context.Advance();
        context.Expect(TokenType.Keyword, TokenSubType.Into);
        context.Advance();

        context.Expect(TokenType.Identifier, TokenSubType.Column);
        currentQuery.TableName = context.Pick().Lexeme;
        context.Advance();

        var columns = ParseColumns(context);

        context.Expect(TokenType.Keyword, TokenSubType.Values);
        context.Advance();

        var values = ParseValues(context);

        if (columns.Count != values.Count)
            throw new Exception("Number of columns and values must match.");

        for (int i = 0; i < columns.Count; i++)
        {
            currentQuery.Values.Add(columns[i], values[i]);
        }

        return currentQuery;
    }

    private List<string> ParseColumns(ParserContext context)
    {
        var columns = new List<string>();
        context.Expect(TokenType.Punctuation, TokenSubType.OpenParenthesis);
        context.Advance();
        while (!context.IsAtEnd && context.Pick().SubType != TokenSubType.CloseParenthesis)
        {
            context.Expect(TokenType.Identifier, TokenSubType.Column);
            columns.Add(context.Pick().Lexeme);
            context.Advance();
        }
        context.Expect(TokenType.Punctuation, TokenSubType.CloseParenthesis);
        context.Advance();
        return columns;
    }

    private List<string> ParseValues(ParserContext context)
    {
        var values = new List<string>();
        context.Expect(TokenType.Punctuation, TokenSubType.OpenParenthesis);
        context.Advance();
        while (!context.IsAtEnd && context.Pick().SubType != TokenSubType.CloseParenthesis)
        {
            context.Expect(TokenType.Identifier, TokenSubType.String, TokenSubType.Number);
            values.Add(context.Pick().Lexeme);
            context.Advance();
        }
        context.Expect(TokenType.Punctuation, TokenSubType.CloseParenthesis);
        context.Advance();
        return values;
    }
}
