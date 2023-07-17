using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class Emails
    {
        [Key]
        public int TokenId { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool Status { get; set; }
    }
}
