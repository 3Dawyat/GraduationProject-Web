
namespace BL.Models.ViewModels.WebModels
{
    public class RevealModel
    {
        public DateTime InvoiceDate => DateTime.UtcNow.ToLocalTime();
      
        [Display(Name = "Patient")]
        public string PatientId { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Requirements And Notes")]
        public string? Notes { get; set; }
        public string Diagnosis { get; set; }
        public string OrderType => "1";
        public DateTime Date =>DateTime.UtcNow.ToLocalTime();
        [Display(Name = "Next Visit")]
        public DateTime NextVisit { get; set; }=DateTime.UtcNow.AddDays(7);
        [MaxLength(7, ErrorMessage = Errors.MaxLength), Display(Name = "Blood Pressure"),
            RegularExpression(RegexPatterns.Pressure, ErrorMessage = Errors.InvalidPressure)]
        public string BloodPressure { get; set; } 
        [Range(1,500, ErrorMessage = Errors.MaxLength)]
        public int Diabites { get; set; } 
        [Range(1, 50, ErrorMessage = Errors.MaxLength), Display(Name = "Body Tempreature")]
        public int BodyTempreature { get; set; } 
      
       public IEnumerable<InvoiceItem>? Items { get; set; }
     //   [Display(Name = "Customer")]
        public IEnumerable<ApplicationUser>? Users { get; set; }
        public IEnumerable<VwItemsWithUnits>? Medicine { get; set; }

    }
}
