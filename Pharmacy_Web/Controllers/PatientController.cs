using BL.Filters;

namespace Pharmacy_Web.Controllers
{
    [Authorize(Roles = AppRoles.Doctor)]
    public class PatientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IService<VwRoshetaHead> _rohetaHead;

        public PatientController(UserManager<ApplicationUser> userManager, IService<VwRoshetaHead> rohetaHead)
        {
            _userManager = userManager;
            _rohetaHead = rohetaHead;
        }


        public async Task<IActionResult> Index()
        {
            var patients= await _userManager.GetUsersInRoleAsync(AppRoles.Patient);
            return View(patients.OrderBy(a=>a.Code));
        } 
        public async Task<IActionResult> PatientProfile(string patientId)
        {
            var model = new PatientProfileModel
            {
                Profile=await _userManager.FindByIdAsync(patientId),
                Reveals=await _rohetaHead.GetListBy (a=>a.PatientId== patientId),
            };
            return View(model);
        }
        [HttpGet, AjaxOnly]
        public IActionResult Add()
        {
            return PartialView("_PatientForm",new RegisterModel());
        }
        [ValidateAntiForgeryToken, HttpPost, ValidateModelState]
        public async Task<IActionResult> Add(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email.ToLower()) is not null)
                return BadRequest(Errors.DuplicatedData);

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var camelCaseAddress = textInfo.ToTitleCase(model.Address);
            var patients = await _userManager.GetUsersInRoleAsync(AppRoles.Patient);

            var lastUser = patients.OrderByDescending(a => a.Code).FirstOrDefault();
            var user = new ApplicationUser
            {
                Address = camelCaseAddress,
                UserName = model.Email.Substring(0, model.Email.IndexOf('@')).ToLower(),
                Email = model.Email.ToLower(),
                FullName = model.FullName,
                Pass = model.Password!,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                Code = lastUser!.Code + 1,
                Age = model.Age,
            };


            var result = await _userManager.CreateAsync(user, user.Pass);

            if (!result.Succeeded)
            {
                var errors = string.Join("\n     ", result.Errors.Select(a => a.Description));
                return BadRequest(errors);
            }
            await _userManager.AddToRoleAsync(user, "Patient");
            return Ok();
        }
    }
}