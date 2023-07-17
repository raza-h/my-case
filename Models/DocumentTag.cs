using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class DocumentTag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Document Name Requird")]
        [RegularExpression(@"^.{3,}$", ErrorMessage = "Minimum 3 characters required")]
        public string DocumentTagName { get; set; }
        public int? FirmId { get; set; }
        public string UserId { get; set; }
    }
}
