using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsolCase.Models
{
    public enum TransactionType
    {
        Credit = 1,
        Debit = 2,
    }
    public class Transactions
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public TransactionType transactionType { get; set; }
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public int DetailAccountId { get; set; }
        public string ClosingBalance { get; set; }
        public string AccountType { get; set; }
        public string OpeningBalance { get; set; }
        [NotMapped]
        public double NumAmountCredit { get; set; }
        [NotMapped]
        public double NumAmountDebit { get; set; }
        [NotMapped]
        public PaymentType Paymenttype { get; set; }
    }
}
