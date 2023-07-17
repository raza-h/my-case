using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string ImagePath { get; set; }
        public byte[] Image { get; set; }
        public string Contact { get; set; }
        public string ContactId { get; set; }
        public string GroupName { get; set; }
        public string UserImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsRead { get; set; }
        public bool Status { get; set; }
        public bool IsGroupMessage { get; set; }
        public int? GroupId { get; set; }
        public int UnreadCount { get; set; }
        public bool IsArchived { get; set; }
    }
}
