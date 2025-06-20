using System.Text.Json.Serialization;
using Domain.Lexer;

namespace Domain.Parser;


[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Token), "token")]
[JsonDerivedType(typeof(Operation), "operation")]
public abstract record Expression;

public record Operation(Expression Operator, List<Expression> Operands) : Expression;
