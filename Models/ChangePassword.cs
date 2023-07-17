using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Please Enter Password.")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string PasswordHash { get; set; }
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        [Required(ErrorMessage = "Please Enter Confirm Password.")]
        [Compare("PasswordHash", ErrorMessage = "Password & Confirm Password does not match")]
        public string ConfirmPasswordHash { get; set; }
    }
}
