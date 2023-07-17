using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
  
    public class RequestUsers
    {
       
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string RoleName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string ImagePath { get; set; }
        public int? PlanID { get; set; }
        public VerificationStatus VerificationStatus { get; set; }
        public bool? Status { get; set; }



    }
}
