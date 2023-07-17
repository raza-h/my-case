using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class AspNetRoles
    {
    
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Please Enter Role Name")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string Name { get; set; }
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public bool Status { get; set; }
    }
}
