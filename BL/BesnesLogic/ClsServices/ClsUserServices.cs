namespace BL.BesnesLogic.ClsServices
{
    public class ClsUserServices : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManegar;
        private readonly JWTModel _jwt;
        private readonly IService<TbCustomers> _customers;

        public ClsUserServices(UserManager<ApplicationUser> user, RoleManager<IdentityRole> rolemanegar, IOptions<JWTModel> jwt, IService<TbCustomers> customers)
        {
            _roleManegar = rolemanegar;
            _jwt = jwt.Value;
            _userManager = user;
            _customers = customers;
        }
        public async Task<string> AddRoleToUserAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return "Invaled User Name !";
            if (!await _roleManegar.RoleExistsAsync(model.RoleName))
                return "Invaled Role Name";
            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                return "User already in this role !";
            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            return result.Succeeded ? $"Succeded ! Added {user.FullName} to Role {model.RoleName}" : "Something went wrong";
        }
        public async Task<AutheModel> RefreshTokenAsync(string token)
        {
            var authModel = new AutheModel();
            var user = await Task.Run(() => _userManager.Users.SingleOrDefault(u => u.RefreshToken!.Any(t => t.Token == token)));
            if (user == null)
            {
                authModel.Massage = "Invalid token";
                return authModel;
            }
            var refreshToken = user.RefreshToken!.FirstOrDefault(t => t.Token == token);

            if (!refreshToken!.IsActive)
            {
                authModel.Massage = "Inactive token";
                return authModel;
            }

            refreshToken.RevokedOn = DateTime.UtcNow.ToLocalTime();
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken!.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            var jwtToken = await CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.FullName = user.FullName;
            authModel.Email = user.Email;
            authModel.UserId = user.Id;
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roales = roles.ToList();
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authModel;
        }
        public async Task<AutheModel> GetTokenAsync(LoginModel model)
        {
            var aouthModel = new AutheModel();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                aouthModel.Massage = "Invaled Data !";
                return aouthModel;
            }
            var rols = await _userManager.GetRolesAsync(user);
            if (!rols.Any(a=>a== AppRoles.User) && !rols.Any(a => a == AppRoles.Patient))
            {
                aouthModel.Massage = "User is not authorized !";
                return aouthModel;
            }

            var JwtSecuretytoken = await CreateJwtToken(user);

            var customer = await _customers.GetObjectBy(a => a.IdentityId == user.Id);
            aouthModel.UserId = user.Id;
            aouthModel.IsAuthenticated = true;
            aouthModel.Roales = rols.ToList();
            aouthModel.Token = new JwtSecurityTokenHandler().WriteToken(JwtSecuretytoken);
            aouthModel.FullName = user.FullName;
            aouthModel.Email = user.Email;
            aouthModel.UserId = user.Id;
            aouthModel.CustomerId = (customer == null) ? 1 : customer.Id;
            if (user.RefreshToken!.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshToken!.FirstOrDefault(a => a.IsActive);
                aouthModel.RefreshToken = activeRefreshToken!.Token;
                aouthModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                aouthModel.RefreshToken = refreshToken.Token;
                aouthModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshToken!.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }
            return aouthModel;
        }
        public async Task<AutheModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email.ToLower()) is not null)
                return new AutheModel { Massage = "User is alrady Registered !" };
            var patients = await _userManager.GetUsersInRoleAsync(AppRoles.Patient);
            var lastUser = patients.OrderByDescending(a => a.Code).FirstOrDefault();
            var user = new ApplicationUser
            {
                Address = model.Address,
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                Pass = model.Password,
                PhoneNumber = model.PhoneNumber,
                NormalizedEmail = model.Email.ToUpper(),
                Gender = model.Gender,
                Code = lastUser!.Code + 1,
                Age = model.Age,
            };


            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var Errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    Errors += $"- {error.Description}\n";
                }
                return new AutheModel { Massage = Errors };
            }
            var customer = new TbCustomers
            {
                CreditLimit = 0,
                IsActive = true,
                Name = model.FullName,
                Address = model.Address,
                Phone = model.PhoneNumber,
                IdentityId = user.Id,
                Age = model.Age,
                Gender = model.Gender,
                Note = "Mobile User"
            };
            await _customers.Add(customer);

            await _userManager.AddToRoleAsync(user, model.Role);
            var JwtSecuretytoken = await CreateJwtToken(user);
            return new AutheModel
            {
                IsAuthenticated = true,
                Roales = new List<string> { model.Role },
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecuretytoken),
                UserId = user.Id,
                CustomerId = customer.Id,
                FullName = user.FullName,
                Email = user.Email,

            };
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.ToLocalTime().AddHours(_jwt.DurationInHours),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await Task.Run(() => _userManager.Users.SingleOrDefault(u => u.RefreshToken!.Any(t => t.Token == token)));
            if (user == null)
                return false;
            var refreshToken = user.RefreshToken!.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
                return false;
            refreshToken.RevokedOn = DateTime.UtcNow.ToLocalTime();
            await _userManager.UpdateAsync(user);
            return true;
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.ToLocalTime().AddHours(10),
                CreatedOn = DateTime.UtcNow.ToLocalTime()
            };
        }
    }
}
