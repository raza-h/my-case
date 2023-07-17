using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class DocumentSign
    {
        public int Id { get; set; }
        public string RecipientId { get; set; }
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentStatus { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientName { get; set; }
        public string DocumentString { get; set; }
        public string DocumentSavedPath { get; set; }
        public DateTime SignRequestedDate { get; set; }
        public DateTime SignCompletedDate { get; set; }
        [NotMapped]
        public IFormFile Document { get; set; }
        [NotMapped]
        public string UsersList { get; set; }
        [NotMapped]
        public string AccessToken { get; set; }

    }
}
