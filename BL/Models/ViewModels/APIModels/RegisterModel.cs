

using UoN.ExpressiveAnnotations.NetCore.Attributes;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace BL.Models.ViewModels.APIModels
{
    public class RegisterModel
    {
        public string? Id { get; set; }
        [MaxLength(100, ErrorMessage = Errors.MaxLength),Display(Name = "Full Name")]
        public string FullName { get; set; }

        [RegularExpression(RegexPatterns.Gender, ErrorMessage = Errors.Gender)]
        public string Gender { get; set; }
        
        [Range(1,99, ErrorMessage = Errors.MaxLength),
            RegularExpression(RegexPatterns.Age, ErrorMessage = Errors.Age)]
        public int Age { get; set; }

        [MaxLength(200, ErrorMessage = Errors.MaxLength), EmailAddress]
        public string Email { get; set; }


        [RegularExpression(RegexPatterns.MobileNumber, ErrorMessage = Errors.InvalidMobileNumber),Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [RequiredIf("Id==null", ErrorMessage = Errors.RequiredField)]
        public string? Role { get; set; }


        [DataType(DataType.Password),
           StringLength(100, ErrorMessage = Errors.MaxMinLength, MinimumLength = 8),
           RegularExpression(RegexPatterns.Password, ErrorMessage = Errors.WeakPassword)]
        [RequiredIf("Id==null",ErrorMessage =Errors.RequiredField)]
        public string? Password { get; set; }


        [DataType(DataType.Password), Display(Name = "Confirm Password"),
            Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch)]
        [RequiredIf("Id==null", ErrorMessage = Errors.RequiredField)]
        public string? ConfirmPassword { get; set; }

    }
}
