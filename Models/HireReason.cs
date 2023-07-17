using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class HireReason
    {
        public int ReasonId { get; set; }
   
        public string ReasonName { get; set; }
        public int? FirmId { get; set; }
        public string UserId { get; set; }
    }
}
