using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Database;

public class TableMeta
{
    public required string Name { get; set; }
    public List<ColumnDefinition> Columns { get; set; } = [];
}
