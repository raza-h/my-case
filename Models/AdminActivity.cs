using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class AdminActivity
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Date { get; set; }
        public string OperationName { get; set; }
        public string ActionDetail { get; set; }
        public string DateFormat { get; set; }
    }
}
