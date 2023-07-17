using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace AbsolCase.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Please valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Password.")]
        public string Password { get; set; }
    }
}
