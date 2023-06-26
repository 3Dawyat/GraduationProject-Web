namespace BL.Models.ViewModels.WebModels
{
    public class DashboardModel
    {
        public IEnumerable<VwRoshetaHead> RoshetaHead { get; set; }
        public int RevealedCount { get; set; }
        public int PatientCount { get; set; }
        public int TodayRevealed { get; set; }
    }
}
