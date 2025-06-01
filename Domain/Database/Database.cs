using System.Threading.Tasks;

namespace Domain.Database;

public static class Database
{
    public static Dictionary<string, Table> Tables { get; set; } = [];

    private static readonly string path = "D:/.Sync/db/Database";

    static Database()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        LoadTables();
    }

    private static void LoadTables()
    {
        var tablesPaths = Directory.GetDirectories(path);
        foreach (var path in tablesPaths)
        {
            var table = new Table(path);
            Tables.Add(table.Meta.Name, table);
        }
    }

    public static Table GetTable(string tableName)
    {
        if (Tables.TryGetValue(tableName, out var table))
        {
            return table;
        }

        throw new Exception($"Table '{tableName}' not found.");
    }

    public static async Task CreateTableAsync(TableMeta meta)
    {
        if (Tables.ContainsKey(meta.Name))
        {
            throw new Exception($"Table '{meta.Name}' already exists.");
        }

        var table = await Table.CreateAsync(Path.Combine(path, meta.Name), meta);
        Tables.Add(meta.Name, table);
    }
}
