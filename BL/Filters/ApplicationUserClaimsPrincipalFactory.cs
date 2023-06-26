namespace Pharmacy_Web.Filters
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory
            (UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options) 
            : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            if (user is not null)
            {
                var identity = await base.GenerateClaimsAsync(user);
                identity.AddClaim(new Claim("FullName", user.FullName));
                identity.AddClaim(new Claim("Address", user.Address!));
                identity.AddClaim(new Claim("PhoneNumber", user.PhoneNumber));
                return identity;
            }
            return new ClaimsIdentity();
        }

    }
}
