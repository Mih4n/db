using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Queries;

public class InsertQuery : IQuery
{
    public string? TableName { get; set; }
    public Dictionary<string, object> Values { get; set; } = [];
}
