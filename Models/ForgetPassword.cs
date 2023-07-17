using System.ComponentModel.DataAnnotations;

namespace AbsolCase.Models
{
    public class ForgetPassword
    {
        [Required(ErrorMessage = "Please enter email address")]
        [EmailAddress(ErrorMessage = "Please valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Password must contain at least six characters, one number, both lower and uppercase letters and a special character")]

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Confirm Password.")]
        [Compare("Password", ErrorMessage = "Password & Confirm Password does not match")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
