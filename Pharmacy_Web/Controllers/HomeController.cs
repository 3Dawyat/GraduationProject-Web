
namespace Pharmacy_Web.Controllers
{
    [Authorize(Roles =AppRoles.Doctor)]
    public class HomeController : Controller
    {
        private readonly IService<VwRoshetaHead> _rohetaHead;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IService<VwItemsWithUnits> _items;
        public static List<VwItemsWithUnits> Medicines;
        public HomeController(IService<VwRoshetaHead> rohetaHead, UserManager<ApplicationUser> userManager, IService<VwItemsWithUnits> items)
        {
            _rohetaHead = rohetaHead;
            _userManager = userManager;
            _items = items;
            Medicines = new List<VwItemsWithUnits>();
        }

        public IActionResult Patient()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            BackgroundJob.Enqueue(() => SetMedicines());
           // Medicines = await _items.GetAll();
            var usersInRole = await _userManager.GetUsersInRoleAsync(AppRoles.Patient);
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = new DashboardModel
            {
                RoshetaHead = await _rohetaHead.GetListBy(a=>a.DoctorId== doctorId),
                RevealedCount = await _rohetaHead.CountWhere(a => a.DoctorId == doctorId),
                PatientCount = usersInRole.Count(),
                TodayRevealed = await _rohetaHead.CountWhere(a => a.DoctorId == doctorId &&a.Date >= DateTime.Today.AddDays(-1) && a.Date < DateTime.Today.AddDays(1))
            }; 
            ;
            return View(model);
        }
        public async Task SetMedicines()
        {
            Medicines = await _items.GetAll();
        }

    }
}
