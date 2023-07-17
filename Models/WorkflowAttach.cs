using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class WorkflowAttach
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? CaseId { get; set; }
        public int? WorkflowId { get; set; }
        public int? FirmId { get; set; }
        [NotMapped]
        public string WorkflowName { get; set; }
    }
}
