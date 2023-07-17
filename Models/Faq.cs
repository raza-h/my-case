using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class Faq
    {
    
        public int Id { get; set; }
        [Required(ErrorMessage = "Question Title Required")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string Question { get; set; }
        [Required(ErrorMessage = "Answer Required")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string Answer { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
