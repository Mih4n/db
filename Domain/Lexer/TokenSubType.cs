namespace Domain.Lexer;

public enum TokenSubType
{
    Any,
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
    // Punctuation
    OpenParenthesis,
    CloseParenthesis,
    // Identifiers
    Column,
    Table,
    Number,
    Operator,
    String
}
