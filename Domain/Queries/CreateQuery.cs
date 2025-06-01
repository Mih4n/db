namespace Domain.Queries;

public class CreateQuery : IQuery
{
    public string TableName { get; set; } = string.Empty;
    public List<ColumnDefinition> Columns { get; set; } = new List<ColumnDefinition>();
}