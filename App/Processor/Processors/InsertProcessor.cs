using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using App.Contracts.Processor;
using Domain.Database;
using Domain.Queries;

namespace App.Processor.Processors;

public class InsertProcessor : IQueryProcessor
{
    public Type type => typeof(InsertQuery);

    public async Task<object> ProcessAsync(IQuery query)
    {
        if (query is not InsertQuery insertQuery)
            throw new ArgumentException("Unsupported query type", nameof(query));

        if (insertQuery.TableName == null)
            throw new ArgumentNullException(nameof(insertQuery.TableName), "Table name cannot be null");

        var table = Database.GetTable(insertQuery.TableName);

        if (table == null)
            throw new ArgumentException($"Table '{insertQuery.TableName}' does not exist.", nameof(insertQuery.TableName));

        table
            .Meta
            .Columns
            .Where(c => !insertQuery
                .Values
                .Any(v =>
                    {
                        return c.Name == v.Key;
                    }
                )
            )
            .ToList()
            .ForEach(c =>
                insertQuery.Values.Add(c.Name, string.Empty)
            );

        Console.WriteLine("Inserting values into table: " + JsonSerializer.Serialize(insertQuery.Values));

        await table.InsertRowAsync(insertQuery.Values);

        return $"Inserted {insertQuery.Values.Count} rows into table '{insertQuery.TableName}'.";
    }
}
