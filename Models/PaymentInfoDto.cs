using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class PaymentInfoDto
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public string Price { get; set; }
        public string PlanName { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public int? SubsciptionId { get; set; }
        public string FromScreen { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public UserDto UserData { get; set; }
    }
}
