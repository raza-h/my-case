using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please select client")]
        public string ClientId { get; set; }
        public int CaseId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string ExpenseType { get; set; }
        public string PaymentMode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InvoiceNo { get; set; }
        public string CheckNumber { get; set; }
        public string CheckTitle { get; set; }
        public DateTime? CheckDate { get; set; }
        public string CheckImagePath { get; set; }
        public bool IsCash { get; set; }
        public bool IsCredit { get; set; }
        public IFormFile Image { get; set; }
        public byte[] File { get; set; }
        public int? FirmId { get; set; }


    }
}
