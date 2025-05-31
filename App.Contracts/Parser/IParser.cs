using Domain.Queries;

namespace App.Contracts.Parser;

public interface IParser
{
    IQuery Parse();
}
