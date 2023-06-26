using BL.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy_Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;


        public AccountController(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = _mapper.Map<RegisterModel>(user);
            return View(model);
        }
        [HttpGet, AjaxOnly]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound($"No User With Same Id");

            var model = _mapper.Map<RegisterModel>(user);
            return PartialView("~/Views/Patient/_PatientForm.cshtml", model);
        }
        [HttpPost, ValidateModelState, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null)
                return BadRequest();
            user = _mapper.Map(model, user);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));
            return Ok();
        }
    }
}
