using App.Contracts.Lexer;
using Domain.Lexer;

namespace App.Lexer.TokenHandlers;

public class WordHandler : ITokenHandler
{
    public bool CanHandle(char current)
        => IsWord(current);

    private static readonly Dictionary<string, TokenSubType> keywords = new()
    {
        ["select"] = TokenSubType.Select,
        ["from"] = TokenSubType.From,
        ["where"] = TokenSubType.Where
    };

    public Token? Handle(LexerContext context)
    {
        var word = context.ReadWhile(c => IsWord(c) || char.IsDigit(c));
        var lowerWord = word.ToLowerInvariant();
        if (keywords.TryGetValue(lowerWord, out var subType))
        {
            return new Token(TokenType.keyword, subType, word);
        }
        else
        {
            return new Token(TokenType.Identifier, TokenSubType.Word, word);
        }
    }

    private bool IsWord(char c) => char.IsLetter(c) || c == '_';
}
