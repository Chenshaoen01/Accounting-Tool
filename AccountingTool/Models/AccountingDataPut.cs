namespace AccountingTool.Models
{
    public class AccountingDataPut
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int Label { get; set; }

        public DateTime Time { get; set; }

        public int Price { get; set; }
    }
}
