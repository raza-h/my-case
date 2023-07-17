using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Areas.Cases.Models
{
    public class ContactGroup
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Group Name Required")]
        public string ContactGroupName { get; set; }
    }
}
