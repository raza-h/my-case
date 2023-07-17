using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class TimeEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimeEntryID { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }
        public int? FirmId { get; set; }
        public int? CaseId { get; set; }
        public int? ActivityId { get; set; }
        public int? Rate { get; set; }
        public double? Total { get; set; }
        public double? Duration { get; set; }
        public string UserId { get; set; }
        public string RateType { get; set; }
        public bool IsBillable { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public List<CustomField> customField { get; set; }
        [NotMapped]
        public List<CFieldValue> cfieldValue { get; set; }
        [NotMapped]
        public string Casename { get; set; }
        [NotMapped]
        public string Username { get; set; }
        [NotMapped]
        public string Activityname { get; set; }

    }
}
