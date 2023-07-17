using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class EventsViewModel
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Start { get; set; }
        public string  starDate{ get; set; }
        public string End { get; set; }
        public string UserId { get; set; }
        public bool? AllDay { get; set; }
    }
}
