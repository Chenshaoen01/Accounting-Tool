using System;
using System.Collections.Generic;

namespace AccountingTool.Models;

public partial class AccountingData
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int Label { get; set; }

    public byte[] Time { get; set; } = null!;

    public int Price { get; set; }
}
