namespace Domain.Lexer;

public static class TokenBindingPowerExtension
{
    public static (int leftBp, int rightBp) GetBindingPower(this Token token)
    {
        return token.Type switch
        {
            TokenType.Keyword => (1, 2),
            TokenType.Operator => token.SubType switch
            {
                TokenSubType.Addition => (10, 11),
                TokenSubType.Subtraction => (10, 11),
                TokenSubType.Multiplication => (20, 21),
                TokenSubType.Division => (20, 21),
                _ => (0, 0)
            },
            TokenType.Punctuation => token.SubType switch
            {
                _ => (0, 0)
            },
            _ => throw new ArgumentException($"Token type {token.Type} does not have binding power", nameof(token))
        };
    }
}
