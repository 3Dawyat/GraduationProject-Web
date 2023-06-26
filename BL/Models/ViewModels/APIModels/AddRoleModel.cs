namespace BL.Models.ViewModels.APIModels
{
    public class AddRoleModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
