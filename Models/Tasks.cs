using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public enum Priority
    {
        NoPriority = 1,
        Low = 2,
        Medium = 3,
        High = 4
    }
    public enum TaskStatus
    {
        Active = 1,
        InProgress = 2,
        Completed = 3,
        OnHold = 4,
        Dismissed = 5,
        Closed = 6
    }
    public class Tasks
    {
        [Key]
        public int Id { get; set; }


        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; }
        public TaskStatus Status { get; set; }
     
        public string Description { get; set; }
        public string ClientId { get; set; }
        public string StaffId { get; set; }
        public int? CaseId { get; set; }
        public string CaseName { get; set; }
        public int? FirmId { get; set; }
        public int? WorkflowId { get; set; }

    }
}
