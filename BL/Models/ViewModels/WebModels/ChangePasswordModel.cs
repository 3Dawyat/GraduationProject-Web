using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace BL.Models.ViewModels.WebModels
{
    public class ChangePasswordModel
    {
        public string Id { get; set; } = null!;

        [DataType(DataType.Password),
            StringLength(100, ErrorMessage = Errors.MaxLength, MinimumLength = 8),
            RegularExpression(RegexPatterns.Password, ErrorMessage =Errors.WeakPassword)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password), Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
