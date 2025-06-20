using System.Reflection;
using System.Text.Json;
using App.Contracts.Lexer;
using Domain.Lexer;

namespace App.Lexer;

public class Lexer
{
    private readonly List<ITokenHandler> handlers = [];

    public Lexer()
    {
        Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ITokenHandler).IsAssignableFrom(t))
            .ToList()
            .ForEach(t =>
            {
                var handler = (ITokenHandler?)Activator.CreateInstance(t);
                if (handler != null)
                    handlers.Add(handler);
            });
    }
    
    public List<Token> Tokenize(string text)
    {
        var context = new LexerContext(text);

        var tokens = new List<Token>();

        while (!context.IsAtEnd)
        {
            SkipPunctuation(context);

            if (context.IsAtEnd)
                break;

            char current = context.Peek();
            ITokenHandler? handler = handlers.FirstOrDefault(h => h.CanHandle(current));

            if (handler == null)
                throw new Exception($"Unexpected character at position {context.Position}: {current}");

            var token = handler.Handle(context);

            if (token == null)
                throw new Exception($"Failed to handle token at position {context.Position}: {current}");

            tokens.Add(token);

            if (token.Type == TokenType.Keyword)
                context.Keyword = token;

            if (token.Type == TokenType.Punctuation)
                context.Keyword = null;
        }

        tokens.Add(new Token(TokenType.Keyword, TokenSubType.End, string.Empty));

        return tokens;
    }

    private HashSet<char> SkippingPunctuation = [
        ' ',
        ',',
        ';',
        '\n',
        '\r',
    ];

    private void SkipPunctuation(LexerContext context)
    {
        while (
            !context.IsAtEnd &&
            SkippingPunctuation.Contains(context.Peek())
        )
        {
            context.Advance();
        }
    } 
}