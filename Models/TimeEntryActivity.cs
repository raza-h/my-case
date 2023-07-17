using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class TimeEntryActivity
    {
        [Key]
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string UserId { get; set; }
        public int? FirmId { get; set; }

        [NotMapped]
        public List<CustomField> customField { get; set; }
        [NotMapped]
        public List<CFieldValue> cfieldValue { get; set; }

    }
}
