using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class BillingMethod
    {
        public int Id { get; set; }
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        [Required(ErrorMessage= "Billing Name is Required")]
        public string Name { get; set; }
        public int? FirmId { get; set; }
        public string UserId { get; set; }
    }
}
