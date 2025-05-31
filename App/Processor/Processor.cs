using Domain.Queries;

namespace App.Processor;

public class Processor
{
    Dictionary<string, Row[]> tables = new()
    {
        ["users"] = [
            new Row { ["id"] = 1, ["name"] = "Alice", ["age"] = 30 },
            new Row { ["id"] = 2, ["name"] = "Bob", ["age"] = 25 },
            new Row { ["id"] = 3, ["name"] = "Charlie", ["age"] = 35 }
        ],
        ["orders"] = [
            new Row { ["id"] = 1, ["user_id"] = 1, ["amount"] = 100 },
            new Row { ["id"] = 2, ["user_id"] = 2, ["amount"] = 200 },
            new Row { ["id"] = 3, ["user_id"] = 3, ["amount"] = 300 }
        ],
        ["products"] = [
            new Row { ["id"] = 1, ["name"] = "Laptop", ["price"] = 1000 },
            new Row { ["id"] = 2, ["name"] = "Smartphone", ["price"] = 500 },
            new Row { ["id"] = 3, ["name"] = "Tablet", ["price"] = 300 }
        ]
    };

    public object Process(IQuery query)
    {
        if (query is not SelectQuery selectQuery)
            throw new ArgumentException("Unsupported query type", nameof(query));

        if (selectQuery.Table == null)
            throw new ArgumentException("Table name cannot be null", nameof(selectQuery.Table));

        if (!tables.TryGetValue(selectQuery.Table, out var rows))
            throw new ArgumentException($"Table '{selectQuery.Table}' does not exist", nameof(selectQuery.Table));

        var result = new List<Row>();

        foreach (var row in rows)
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
