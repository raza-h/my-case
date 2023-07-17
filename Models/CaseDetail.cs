using MyCaseApi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public enum Stages
    {
        Discovery = 1,
        InTrial = 2,
        OnHold = 3
    }
    public class CaseDetail
    {
        public int Id { get; set; }
        public string CaseName { get; set; }
        public int? CaseNumber { get; set; }
        public int? PracticeArea { get; set; }
        public Stages? CaseStage { get; set; }
        public DateTime DateAppend { get; set; }
        public string Office { get; set; }

        public string Description { get; set; }
        public DateTime? StatueOfLimitation { get; set; }
        public string ConflictCheckNotes { get; set; }
        public string JobTitle { get; set; }
        public int? CaseRate { get; set; }
        public int? BillingContact { get; set; }
        public int? BillingMethod { get; set; }
        public string LeadAttorney { get; set; }
        public string OriginatingLeadAttorney { get; set; }
        public string CaseAddedBy { get; set; }
        public string ClientName { get; set; }
        [NotMapped]
        public string BillingContactList { get; set; }
        [NotMapped]
        public Firm firm { get; set; }
        [NotMapped]
        public string PracticeAreaName { get; set; }
        [NotMapped]
        public ClientLocation ClientLocation { get; set; }
        [NotMapped]
        public Contact contact { get; set; }
        public DateTime? DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool IsOpen { get; set; }
        [NotMapped]
        public List<UserAgainstCase> userAgainstCase { get; set; }
        [NotMapped]
        public List<CustomField> customField { get; set; }
        [NotMapped]
        public List<CFieldValue> cfieldValue { get; set; }
        [NotMapped]
        public List<CustomField> customFieldTime { get; set; }
        [NotMapped]
        public List<WorkflowAttach> workflowAttach { get; set; }

    }
}
