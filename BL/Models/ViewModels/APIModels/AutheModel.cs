namespace BL.Models.ViewModels.APIModels
{
    public class AutheModel
    {
        public string UserId { get; set; }
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<string> Roales { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Massage { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
