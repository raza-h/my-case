using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class ChatGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string AdminId { get; set; }
        public List<string> UserIds { get; set; }
        public string ImagePath { get; set; }
        public byte[] Image { get; set; }
        public Message message { get; set; }
    }
}
