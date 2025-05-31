using Domain.Lexer;
using Domain.Parser;
using Domain.Queries;

namespace App.Contracts.Parser;

public interface IQueryParser
{
    public bool CanParse(Token token);
    public IQuery Parse(ParserContext context);
}
