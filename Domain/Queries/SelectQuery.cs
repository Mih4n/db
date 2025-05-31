namespace Domain.Queries;

public class SelectQuery : IQuery
{
    public string? Table { get; set; }
    public Condition? Condition { get; set; }
    public List<string> Columns { get; set; } = [];
}

public class Condition
{
    public required string Column { get; set; }
    public required string Operator { get; set; }
    public required string Value { get; set; }

    public bool Evaluate(object? value)
    {
        if (value == null) return false;

        switch (Operator)
        {
            case "=":
                return value.ToString() == Value;
            case "!=":
                return value.ToString() != Value;
            case ">":
                return Convert.ToDouble(value) > Convert.ToDouble(Value);
            case "<":
                return Convert.ToDouble(value) < Convert.ToDouble(Value);
            case ">=":
                return Convert.ToDouble(value) >= Convert.ToDouble(Value);
            case "<=":
                return Convert.ToDouble(value) <= Convert.ToDouble(Value);
            default:
                throw new Exception($"Unknown operator: {Operator}");
        }
    }
}
