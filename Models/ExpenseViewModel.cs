using System;

namespace AbsolCase.Models
{
    public class ExpenseViewModel
    {
        public int Id { get; set; }
        public string StaffId { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public int CaseId { get; set; }
        public string CaseName { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string ExpenseType { get; set; }
        public string ExpenseTypeName { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentModeName { get; set; }
        public string CreatedBy { get; set; }
        public string InvoiceNo { get; set; }
        public string CheckNumber { get; set; }
        public string CheckTitle { get; set; }
        public DateTime? CheckDate { get; set; }
        public string CreatedDate { get; set; }
        public string CheckImagePath { get; set; }
        public bool IsCash { get; set; }
        public bool IsCredit { get; set; }
    }
}
