
namespace BL.Models.ViewModels.APIModels
{
    [DebuggerDisplay("Id={ItemId}  -  Name={ItemName}  -  Image={ImageName}  -  Price={SalesPrice}")]
    public class ItemModel
    {

        public int ItemId { get; set; }
        public string ItemName { get; set; }=string .Empty;
        public string ImageName { get; set; } = string.Empty;
        public decimal SalesPrice { get; set; }
        public int CategoryId { get; set; }

    }
}
