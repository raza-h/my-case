using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace AbsolCase.Models
{
    public enum ClientPaymentType
    {
        Cash = 1,
        Bank = 2
    }
    public class ClientTransaction
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter amount")]
        [RegularExpression(@"^(?=.*[1-9])(?:[1-9]\d*\.?|0?\.)\d*$", ErrorMessage = "Please enter valid amount.")]
        public string Amount { get; set; }
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PaymentDate { get; set; }
        public string InvoiceNo { get; set; }
        public ClientPaymentType PaymentType { get; set; }
        public string Note { get; set; }
        [Required(ErrorMessage = "Please select client")]
        public int? ContactId { get; set; }
        public bool IsCash { get; set; }
        public bool IsCredit { get; set; }
        [Required(ErrorMessage = "Please enter check number")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string CheckNumber { get; set; }
        [Required(ErrorMessage = "Please enter check title")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string CheckTitle { get; set; }
        [Required(ErrorMessage = "Please select check date")]
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CheckDate { get; set; }
        public string CheckAmount { get; set; }
        [Required(ErrorMessage = "Please select check image")]
        public IFormFile Image { get; set; }
        public byte[] File { get; set; }
        public string UserId { get; set; }
        public string ParentId { get; set; }
        public string ClientName { get; set; }
        public string CheckImagePath { get; set; }
        public Contact Contact { get; set;}
        public User User { get; set; }

    }
}
