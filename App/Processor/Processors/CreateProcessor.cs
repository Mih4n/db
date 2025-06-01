using App.Contracts.Processor;
using Domain.Database;
using Domain.Queries;

namespace App.Processor.Processors;

public class CreateProcessor : IQueryProcessor
{
    public Type type => typeof(CreateQuery);

    public async Task<object> ProcessAsync(IQuery query)
    {
        if (query is not CreateQuery createQuery)
            throw new ArgumentException("Unsupported query type", nameof(query));

        await Database.CreateTableAsync(new TableMeta
            {
                Name = createQuery.TableName,
                Columns = createQuery.Columns.Select(c => new ColumnDefinition
                    {
                        Name = c.Name,
                        Type = c.Type,
                    }
                )
                .ToList()
            }
        );

        return $"Table '{createQuery.TableName}' created successfully.";
    }
}
