using App.Contracts.Processor;
using Domain.Database;
using Domain.Queries;

namespace App.Processor.Processors;

public class SelectProcessor : IQueryProcessor
{
    public Type type => typeof(SelectQuery);
    
    public async Task<object> ProcessAsync(IQuery query)
    {
        if (query is not SelectQuery selectQuery)
            throw new ArgumentException("Unsupported query type", nameof(query));

        if (selectQuery.Table == null)
            throw new ArgumentException("Table name cannot be null", nameof(selectQuery.Table));

        var table = Database.GetTable(selectQuery.Table);

        var result = new List<Row>();

        await foreach (var row in table.ScanRowsAsync())
        {
            if (selectQuery.Condition == null || selectQuery.Condition.Evaluate(row[selectQuery.Condition.Column]))
            {
                var selectedRow = new Row();
                if (selectQuery.Columns.Contains("*"))
                {
                    foreach (var column in row.Keys)
                    {
                        selectedRow[column] = row[column];
                    }
                }
                else
                {
                    foreach (var column in selectQuery.Columns)
                    {
                        if (row.ContainsKey(column))
                            selectedRow[column] = row[column];
                        else
                            throw new ArgumentException($"Column '{column}' does not exist in table '{selectQuery.Table}'", nameof(selectQuery.Columns));
                    }
                }
                result.Add(selectedRow);
            }
        }

        return result;
    }
}
