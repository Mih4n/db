using System.Reflection;
using App.Contracts.Processor;
using Domain.Queries;

namespace App.Processor;

public class Processor
{
    private readonly Dictionary<Type, IQueryProcessor> processors = [];

    public Processor()
    {
        Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                typeof(IQueryProcessor).IsAssignableFrom(t))
            .ToList()
            .ForEach(t =>
            {
                var processor = (IQueryProcessor?)Activator.CreateInstance(t);
                if (processor != null)
                    processors[processor.type] = processor;
            });
    }

    public async Task<object> ProcessAsync(IQuery query)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query), "Query cannot be null");

        if (!processors.TryGetValue(query.GetType(), out var processor))
            throw new ArgumentException($"No processor found for query type {query.GetType()}", nameof(query));

        return await processor.ProcessAsync(query);
    }
}
