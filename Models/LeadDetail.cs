using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public enum CaseStatus
    {
        New = 1,
        Contacted = 2,
        ConsultScheduled = 3,
        Pending = 4
    }
    public enum Office
    {
        Primary = 1
    }
    public class LeadDetail
    {
        public int Id { get; set; }
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }
        public string MidName { get; set; }
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        [Required(ErrorMessage = "LasName is required")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? ZipCode { get; set; }
        public int? Country { get; set; }
        public DateTime? DOB { get; set; }
        public string DriverLicence { get; set; }
        public string DriverLicenceState { get; set; }
        public string LeadDetails { get; set; }
        public DateTime? DateAdded { get; set; }
        public CaseStatus Status { get; set; }
        public int? PracticeAreaId { get; set; }
        public int? PotentialValueCase { get; set; }
        public string AssignTo { get; set; }
        public Office Office { get; set; }
        public string PotentailCaseDescription { get; set; }
        public bool ConflictCheck { get; set; }
        public string ConflictCheckNotes { get; set; }
        public int? RefferelSource { get; set; }
        public int? ContactId { get; set; }
        public int? CompanyId { get; set; }
        public int? LStatusId { get; set; }
        public string LStatusName { get; set; }
    }

}
