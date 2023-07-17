using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class LeadStatus
    {
        [Key]
        public int LStatusId { get; set; }
        public string LStatusName { get; set; }
        public string UserId { get; set; }
        public int? FirmId { get; set; }
    }
}
