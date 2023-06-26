using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.ViewModels.APIModels
{
    public class DetailsItemModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ActiveIngredient { get; set; } = string.Empty;
        public string Pamphlet { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Composition { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public decimal SalesPrice { get; set; }
        public int CategoryId { get; set; }

    }
}
