﻿namespace AccountingTool.Models
{
    public class AccountingDataPost
    {
        public string? Description { get; set; } = "";

        public string Category { get; set; } = null!;

        public int Label { get; set; }

        public DateTime Time { get; set; }

        public int Price { get; set; }
    }
}
