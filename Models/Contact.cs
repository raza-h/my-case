using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3
    }
    public class Contact
    {
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsClientEnable { get; set; }
        public int ContactGroupId { get; set; }
        public string CellPhone { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? ZipCode { get; set; }
        public int? CountryId { get; set; }
        public string JobTitle { get; set; }
        public string Website { get; set; }
        public string Notes { get; set; }
        public int? CompanyId { get; set; }
        public string DrivingLicense { get; set; }
        public string DrivingLicenseState { get; set; }
        public string FaxNumber { get; set; }
        public string UserId { get; set; }
        public string ContactGroupName { get; set; }
        public string CountryName { get; set; }
        public int? FirmId { get; set; }
        [NotMapped]
        public Firm firm { get; set; }
        [NotMapped]
        public List<CustomField> customField { get; set; }

        [NotMapped]
        public List<CFieldValue> cfieldValue { get; set; }
    }
}
