using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.ViewModels
{
    public class EmailModel
    {
        [EmailAddress]
        public string To { get; set; }
        public string Message { get; set; }
    }
}
