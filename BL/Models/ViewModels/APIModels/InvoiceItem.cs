namespace BL.Models.ViewModels.APIModels
{
    public class InvoiceItem
    {
        [Range(1, int.MaxValue)]
        public int ItemId { get; set; }
        [Range(1, int.MaxValue)]
        public decimal Price { get; set; }
        [Range(1, int.MaxValue)]
        public decimal Qty { get; set; }
        [JsonIgnore]
        public string ItemName =>string.Empty;
        public string Notes { get; set; }=string .Empty;
    }
}
