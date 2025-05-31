namespace Domain.Lexer;

public enum TokenSubType
{
    // Special tokens
    End,
    // Keywords selection
    Select,
    From,
    Where,
    // Keywords manipulation
    Insert,
    Update,
    Delete,
    Create,
    Drop,
    // Keywords control flow
    If,
    Else,
    // Identifiers
    Number,
    Operator,
    Word,
    String
}
