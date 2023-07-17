using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class WorkflowBase
    {
        [Key]
        public int WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public int? TaskId { get; set; }
        public int? DocumentId { get; set; }
        public int? CalenderEventId { get; set; }
        public int? FirmId { get; set; }
        [NotMapped]
        public List<Tasks> tasks { get; set; }
        [NotMapped]
        public List<Decuments> documents { get; set; }
        [NotMapped]
        public List<Events> events { get; set; }
    }
}
