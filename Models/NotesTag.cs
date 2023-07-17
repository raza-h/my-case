using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class NotesTag
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Notes Name Required")]
        [RegularExpression(@"^.{3,}$", ErrorMessage = "Minimum 3 characters required")]
        public string NotesTagName { get; set; }
        public int? FirmId { get; set; }
        public string UserId { get; set; }
    }
}
