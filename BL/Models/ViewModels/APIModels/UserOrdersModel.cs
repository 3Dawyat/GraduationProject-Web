
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.Models.ViewModels.APIModels
{
    public class UserOrdersModel
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public List<VwOrderItem> Items { get; set; }
    }
}
