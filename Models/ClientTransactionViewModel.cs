using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class ClientTransactionViewModel
    {
      
        public string Id { get; set; }
        public string Amount { get; set; }
        public string PaymentDate { get; set; }
        public string InvoiceNo { get; set; }
        public string PaymentType { get; set; }
        public string Note { get; set; }
        public int? ContactId { get; set; }
        public string IsCash { get; set; }
        public string IsCredit { get; set; }
        public string CheckNumber { get; set; }
        public string CheckTitle { get; set; }
        public string CheckDate { get; set; }
        public string CheckAmount { get; set; }
        public string CheckImagePath { get; set; }
       
        public string UserId { get; set; }
        public string ParentId { get; set; }
    
        public string ClientName { get; set; }
       
        public string CaseName { get; set; }
     
        public string Contact { get; set; }
        public string CreatedDate { get; set; }
   
        public string User { get; set; }
    }
}
