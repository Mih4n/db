using System.Reflection;
using App.Contracts.Parser;
using Domain.Lexer;
using Domain.Parser;
using Domain.Queries;

namespace App.Lexer;

public class Parser : IParser
{
    private ParserContext context;
    private List<IQueryParser> parsers = [];

    public Parser(Lexer lexer)
    {
        context = new ParserContext(lexer.Tokenize());

        Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                typeof(IQueryParser).IsAssignableFrom(t)
            )
            .ToList()
            .ForEach(t =>
            {
                var parser = (IQueryParser?)Activator.CreateInstance(t);
                if (parser != null)
                    parsers.Add(parser);
            });
    }

    public IQuery Parse()
    {
        var parser = parsers.FirstOrDefault(p => p.CanParse(context.Pick()));
        if (parser == null)
            throw new Exception("No parser found for the current token");
        return parser.Parse(context);
    }
}