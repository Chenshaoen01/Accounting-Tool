using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingTool.Models;

public partial class AccountingData
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Description { get; set; } = "";

    public string Category { get; set; } = null!;

    public int Label { get; set; }

    public DateTime Time { get; set; }

    public int Price { get; set; }
}
