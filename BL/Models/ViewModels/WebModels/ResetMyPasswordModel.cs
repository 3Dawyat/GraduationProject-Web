namespace BL.Models.ViewModels.WebModels
{
    public class ResetMyPasswordModel
    {
        [MaxLength(200, ErrorMessage = Errors.MaxLength), EmailAddress]
        public string Email { get; set; }
    }
}
