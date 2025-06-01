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
        ["where"] = TokenSubType.Where,
        ["insert"] = TokenSubType.Insert,
        ["update"] = TokenSubType.Update,
        ["delete"] = TokenSubType.Delete,
        ["create"] = TokenSubType.Create,
        ["drop"] = TokenSubType.Drop,
        ["if"] = TokenSubType.If,
        ["else"] = TokenSubType.Else
    };

    public Token? Handle(LexerContext context)
    {
        var word = context.ReadWhile(c => IsWord(c) || char.IsDigit(c));
        var lowerWord = word.ToLowerInvariant();
        if (keywords.TryGetValue(lowerWord, out var subType))
            return new Token(TokenType.Keyword, subType, word);

        if (context.Keyword?.SubType == TokenSubType.From)
            return new Token(TokenType.Identifier, TokenSubType.Table, word);

        if (context.Keyword?.SubType == TokenSubType.Create)
            return new Token(TokenType.Keyword, TokenSubType.Table, word);

        if (context.Keyword?.Type == TokenType.Keyword && context.Keyword?.SubType == TokenSubType.Table)
            return new Token(TokenType.Identifier, TokenSubType.Table, word);

        return new Token(TokenType.Identifier, TokenSubType.Column, word);
    }

    private bool IsWord(char c) => char.IsLetter(c) || c == '_';
}
