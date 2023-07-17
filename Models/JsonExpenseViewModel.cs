using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class JsonExpenseViewModel
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string ClientName { get; set; }
        public string CreatedDate { get; set; }
        public string ClientId { get; set; }
        public string CaseId { get; set; }
        public string CaseName { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string ExpenseType { get; set; }
        public string ExpenseTypeName { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentModeName { get; set; }
        public string CreatedBy { get; set; }
        public string InvoiceNo { get; set; }
        public string CheckNumber { get; set; }
        public string CheckTitle { get; set; }
        public string CheckDate { get; set; }
        public string CheckImagePath { get; set; }
        public string IsCash { get; set; }
        public string IsCredit { get; set; }
    }
}
