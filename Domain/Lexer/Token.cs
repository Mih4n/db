using Domain.Parser;

namespace Domain.Lexer;

public record Token(TokenType Type, TokenSubType SubType, string Lexeme) : Expression;
