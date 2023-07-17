using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class Events
    {

        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string ThemeColor { get; set; }
        public string UserId { get; set; }
        public bool? AllDay { get; set; }
        public int? WorkflowId { get; set; }
    }
}
