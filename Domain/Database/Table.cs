using System.Text.Json;

namespace Domain.Database;

public class Table
{
    private readonly string dataPath;
    public TableMeta Meta { get; set; }

    public Table(string directory)
    {
        dataPath = Path.Combine(directory, $"table.csv");
        var metaPath = Path.Combine(directory, $"meta.json");

        if (!File.Exists(metaPath))
            throw new Exception("Metadata not found");
        if (!File.Exists(dataPath))
            throw new Exception("Data file not found");

        Meta = JsonSerializer.Deserialize<TableMeta>(File.ReadAllText(metaPath))!;
    }

    public static async Task<Table> CreateAsync(string directory, TableMeta meta)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        var metaPath = Path.Combine(directory, "meta.json");
        var dataPath = Path.Combine(directory, "table.csv");

        if (File.Exists(metaPath) || File.Exists(dataPath))
            throw new Exception("Table already exists");

        await File.WriteAllTextAsync(metaPath, JsonSerializer.Serialize(meta));
        await File.WriteAllTextAsync(dataPath, string.Join(",", meta.Columns.Select(c => c.Name)) + Environment.NewLine);

        return new Table(directory) { Meta = meta };
    }

    public async IAsyncEnumerable<IDictionary<string, object>> ScanRowsAsync(Func<IDictionary<string, object>, bool>? predicate = null)
    {
        using var reader = new StreamReader(dataPath);
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            var values = line.Split(',');
            var row = Meta.Columns
                .Select((col, i) => new { col.Name, Value = values[i] })
                .ToDictionary(x => x.Name, x => x.Value as object);

            if (predicate == null || predicate(row))
                yield return row;
        }
    }

    public void InsertRow(Dictionary<string, object> row)
    {
        var line = string.Join(",", Meta.Columns.Select(col => row[col.Name]?.ToString() ?? ""));
        File.AppendAllText(dataPath, line + Environment.NewLine);
    }
}

