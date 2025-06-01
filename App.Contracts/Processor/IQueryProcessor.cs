using Domain.Queries;

namespace App.Contracts.Processor;

public interface IQueryProcessor
{
    public Type type { get; }
    public Task<object> ProcessAsync(IQuery query);
}
