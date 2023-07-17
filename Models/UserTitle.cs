using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class UserTitle
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="User title Required")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string UserTitleName { get; set; }
    }
}
