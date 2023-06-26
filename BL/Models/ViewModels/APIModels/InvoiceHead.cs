
namespace BL.Models.ViewModels.APIModels
{
    public class InvoiceHead
    {
        [JsonIgnore]
        public DateTime Date => DateTime.UtcNow.ToLocalTime(); 
      
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }
        public string UserId { get; set; }
        public List<InvoiceItem> Items { get; set; }

    }
}
