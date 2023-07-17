using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public enum Usertype
    {
        Customer = 1,
        Employee = 2,
        Client = 3,
        Sales = 4
    }
    public class FinancialDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Usertype Type { get; set; }
        public string AccountNumber { get; set; }
        public string UserId { get; set; }
        public string ParentId { get; set; }
    }
}
