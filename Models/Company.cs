using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string MainPhone { get; set; }
        public string FaxNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Enter Valid Postal Code e.g It should match 12345 or 12345-6789.")]
        public int? ZipCode { get; set; }
        public int? CountryId { get; set; }
        public string PrivateNotes { get; set; }
        public string CountryName { get; set; }
        public int? FirmId { get; set; }
        [NotMapped]
        public List<CustomField> customField { get; set; }
        [NotMapped]
        public List<CFieldValue> cfieldValue { get; set; }


    }
}
