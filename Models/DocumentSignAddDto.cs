using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class DocumentSignAddDto
    {
        public int Id { get; set; }
        public int RecipientId { get; set; }
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentStatus { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientName { get; set; }
        public DateTime SignRequestedDate { get; set; }
        public DateTime SignCompletedDate { get; set; }
        public IFormFile Document { get; set; }
    }
}
