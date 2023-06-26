namespace BL.Models.ViewModels.WebModels
{
    public class PatientProfileModel
    {
        public IEnumerable<VwRoshetaHead> Reveals { get; set; }
        public ApplicationUser Profile { get; set; }
    }
}
