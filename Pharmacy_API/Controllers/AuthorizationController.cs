using BL.Filters;
using Pharmacy_DomainModels.Models.DB_Models;

namespace Pharmacy_API.Controllers
{
    [Route("/[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
       private readonly IUserServices _user;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public AuthorizationController(IUserServices userServices, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _user = userServices;

            _userManager = userManager;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }
        [HttpPost, ValidateModelState]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            var result = await _user.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Massage);
            return Ok(result);
        }
      
        [HttpPost,ValidateModelState]
        public async Task<IActionResult> LogIn([FromBody] LoginModel model)
        {
            var result = await _user.GetTokenAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Massage);
            return Ok(result);
        }
        [HttpPost, ValidateModelState]
        public async Task<IActionResult> ResetMyPassword([FromBody] ResetMyPasswordModel model)
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
                BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(model.Email, "Password Recovery - Saidaly Tech", body));
                await _userManager.UpdateAsync(user);
                return Ok();
            }
            user.PasswordHash = currentPasswordHash;
            await _userManager.UpdateAsync(user);
            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));
        } 
        [HttpPost, ValidateModelState]
        public  IActionResult SendEmail([FromBody] EmailModel model)
        {
            var filePath = $"{_webHostEnvironment.WebRootPath}/templets/Email.html";
            StreamReader str = new StreamReader(filePath);
            var body = str.ReadToEnd();
            str.Close();
            body = body.Replace("[NewPassword]", model.Message);
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(model.To, "Saidaly Tech", body));
            return Ok();
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
        [HttpPost, ValidateModelState]
        public async Task<IActionResult> EditUser([FromBody] RegisterModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null)
                return BadRequest();
            user = _mapper.Map(model, user);
            var newPass = model.Password;
            var currentPasswordHash = user.PasswordHash;
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, newPass);
            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles[0]!= model.Role)
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                user.Pass = newPass;
                await _userManager.UpdateAsync(user);
                return Ok();
            }
            user.PasswordHash = currentPasswordHash;
            await _userManager.UpdateAsync(user);
            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));
        }

       

    }
}
