using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public enum PaymentType
    {
        Paypal = 1,
        CreditCard = 2,
        BankAccount = 3,
        None = 4
    }
    public class SignupDto
    {
        public UserSignupDto userSignupDto { get; set; }
        public PaymentDto PaymentInfoDto { get; set; }
        public int PricePlanId { get; set; }
        public PricePlan PricePlan { get; set; }
        public byte[] File { get; set; }
    }

    public class UserSignupDto
    {
        [Required(ErrorMessage = "Please Enter First Name.")]
        [RegularExpression(@"^[A-Za-z\s]{3,}$", ErrorMessage = "It should contain minimum three characters and only alphabets are allowed.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Last Name.")]
        [RegularExpression(@"^[A-Za-z\s]{1,}$", ErrorMessage = "Minimum one character and only alphabets are allowed.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter email address")]
        [EmailAddress(ErrorMessage ="Please enter valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*.?&])[A-Za-z\d@$!%*.?&]{6,}$", ErrorMessage = "Password must contain at least six characters, one number, both lower and uppercase letters and a special character")]

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Confirm Password.")]
        [Compare("Password", ErrorMessage = "Password & Confirm Password does not match")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Please Enter Valid Phone Number.")]
        [RegularExpression(@"^\d{7,16}", ErrorMessage = "Please Enter Valid PhoneNumber.")]
       
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please select role.")]
        public string RoleName { get; set; }
        public string Rate { get; set; }
        public string Address { get; set; }
        [RegularExpression(@"^[A-Za-z]{2,30}$", ErrorMessage = "Enter Valid State Name.")]
        public string State { get; set; }
        [RegularExpression(@"^[A-Za-z]{2,20}$", ErrorMessage = "Enter Valid City Name.")]
        public string City { get; set; }
      
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Enter Valid Postal Code e.g It should match 12345 or 12345-6789.")]
        public string PostCode { get; set; }
        public int UserTitleId { get; set; }
        public string ParentId { get; set; }
        public byte[] file { get; set; }
        public IFormFile profile_img { get; set; }
    }

    public class PaymentDto
    {
        public BankInfoDto Bank { get; set; }

        public CreditCardDto CreditCard { get; set; }

        public PaymentType PaymentType { get; set; }
    }
    public class BankInfoDto
    {
        public string FilePath { get; set; }
    }

    public class CreditCardDto
    {
        public string FullName { get; set; }
        public string CardNumber { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public int CVV { get; set; }
    }
}
