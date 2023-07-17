using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public enum SignUpStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
    }

    public enum Status
    {
        Unblock = 1,
        Block = 2
    }
    public class User
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "please enter first name")]
        [RegularExpression(@"^\d*[a-zA-Z]{1,}\d*$", ErrorMessage = "Please avoid white spaces.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "please enter last name")]
        [RegularExpression(@"^\d*[a-zA-Z]{1,}\d*$", ErrorMessage = "Please avoid white spaces.")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Enter Valid Post Code e.g It should match 12345 or 12345-6789.")]
        public string PostCode { get; set; }
        public string ImagePath { get; set; }
        public string Role { get; set; }
        public string newPassword { get; set; }      
        public Status Status { get; set; }
        public SignUpStatus SignUpStatus { get; set; }
        public IFormFile Image { get; set; }
        public byte[] File { get; set; }
        public string ParentId { get; set; }
    }
}
