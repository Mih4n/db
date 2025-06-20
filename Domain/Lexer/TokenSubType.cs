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
    Into,
    Values,
    Update,
    Delete,
    Create,
    Drop,
    // Keywords control flow
    If,
    Else,
    // Punctuation
    Dot,
    Comma,
    Colon,
    Equals,
    Semicolon,
    OpenParenthesis,
    CloseParenthesis,
    // Identifiers
    Word,
    Number,
    String,
    // Operators
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Modulus,
    Or,
    And,
    Equal,
    NotEqual,
    LessThan,
    GreaterThan,
    Assignment
}
