namespace AccountingTool.Models
{
    public class AccountingDataGet
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int Label { get; set; }
        public Label LabelContent { get; set; }

        public DateTime Time { get; set; }

        public int Price { get; set; }
    }
}
