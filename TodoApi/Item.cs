using System;
using System.Collections.Generic;

namespace TodoApi;

public partial class Item
{
    public int id { get; set; }

    public string Name { get; set; } = null!;

    public bool? isComplete { get; set; }
}
