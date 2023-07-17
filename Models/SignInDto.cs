using System.ComponentModel.DataAnnotations;

namespace AbsolCase.Models
{
    public class SignInDto
    {
        [Required(ErrorMessage ="Please enter email")]
        [EmailAddress(ErrorMessage ="Please valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*.?&])[A-Za-z\d@$!%*.?&]{6,}$", ErrorMessage = "Password must contain at least six characters, one number, both lower and uppercase letters and a special character")]
        public string Password { get; set; }
    }
}
