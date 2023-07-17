using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class PracticeArea
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Practice Name Required")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public  string PracticeAreaName { get; set; }
        public int ActiveCases { get; set; }
        public int? FirmId { get; set; }
        public string UserId { get; set; }
    }
}
