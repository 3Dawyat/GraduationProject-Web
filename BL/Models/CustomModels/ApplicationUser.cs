namespace BL.Models.CustomModels
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(200)]
        public string FullName { get; set; }
        [StringLength(200)]
        public string Pass { get; set; }
        public int Code { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string? Address { get; set; }
        public List<RefreshToken>? RefreshToken { get; set; }
    }
}
