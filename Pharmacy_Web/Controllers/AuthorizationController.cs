
using BL.Filters;
using Hangfire;
using System.Text.RegularExpressions;

namespace Pharmacy_Web.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AuthorizationController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Sign()
        {
            return View(new SignModel());
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> SignIn(SignModel model)
        {
            if (string.IsNullOrEmpty(model.Login.Email) || string.IsNullOrEmpty(model.Login.Password))
            {
                ModelState.AddModelError(string.Empty, Errors.InvalidData);
                return View("Sign", model);
            }
            var email = model.Login.Email.ToUpper();
            var user = await _userManager
                .Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, Errors.InvalidData);
                return View("Sign", model);
            }
            if(!await _userManager.IsInRoleAsync(user, AppRoles.Doctor))
            {
                ModelState.AddModelError(string.Empty, Errors.InvalidData);
                return View("Sign", model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Login.Password, true, false);
            if (result.Succeeded)
                return Redirect("/Home/Dashboard");
            else
            {
                ModelState.AddModelError(string.Empty, Errors.InvalidData);
                return View("Sign", model);
            }
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> SignUp(SignModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Register.Email.ToLower()) is not null)
            {
                ModelState.AddModelError(string.Empty, Errors.DuplicatedData);
                return View("Sign", model);
            }

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var camelCaseAddress = textInfo.ToTitleCase(model.Register.Address);

            var user = new ApplicationUser
            {
                Address = camelCaseAddress,
                UserName = model.Register.Email.Substring(0, model.Register.Email.IndexOf('@')).ToLower(),
                Email = model.Register.Email.ToLower(),
                FullName = model.Register.FullName,
                Pass = model.Register.Password!,
                PhoneNumber = model.Register.PhoneNumber,
                Gender = model.Register.Gender,
            };

            var result = await _userManager.CreateAsync(user, user.Pass);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Sign", model);
            }
            await _userManager.AddToRoleAsync(user, AppRoles.Doctor);
            await _signInManager.PasswordSignInAsync(user, user.Pass, true, false);
            return Redirect("/Home/Dashboard");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }
        [AjaxOnly, HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return BadRequest();
            var viewModel = new ChangePasswordModel { Id = user.Id };
            return PartialView("_ChangePassword", viewModel);
        }
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken,Authorize(Roles = AppRoles.Doctor), ValidateModelState]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
         {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null)
                return BadRequest();
            var currentPasswordHash = user.PasswordHash;
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, model.Password);
            if (result.Succeeded)
            {
                user.Pass = model.Password;
                await _userManager.UpdateAsync(user);
                return Ok();
            }
            user.PasswordHash = currentPasswordHash;
            await _userManager.UpdateAsync(user);
            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));
        }
        [HttpGet, AjaxOnly]
        public IActionResult ResetMyPassword()
        {
            return PartialView("_ResetPassword", new ResetMyPasswordModel());
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateModelState]
        public async Task<IActionResult> ResetMyPassword(ResetMyPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return BadRequest();
            var newPass = CreateNewPassword();
            var currentPasswordHash = user.PasswordHash;
            var filePath = $"{_webHostEnvironment.WebRootPath}/templets/Email.html";
            StreamReader str = new StreamReader(filePath);
            var body = str.ReadToEnd();
            str.Close();
            body = body.Replace("[NewPassword]", newPass);

            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, newPass);
            if (result.Succeeded)
            {
                user.Pass = newPass;
                BackgroundJob.Enqueue(()=>  _emailSender.SendEmailAsync(model.Email, "Password Recovery - Saidaly Tech", body));
               await _userManager.UpdateAsync(user);
                return Ok();
            }
            user.PasswordHash = currentPasswordHash;
            await _userManager.UpdateAsync(user);
            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));
        }
        private string CreateNewPassword()
        {
            Regex regex = new Regex(RegexPatterns.Password);
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
              
            while (sb.Length < 8 || !regex.IsMatch(sb.ToString()))
            {
                char c = (char)random.Next(33, 127); // ASCII range from ! to ~
                sb.Append(c);
            }

            string result = sb.ToString();

            return result;

        }
    }
}