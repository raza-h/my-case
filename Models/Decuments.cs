using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class Decuments
    {
        [Key]
        public int Id { get; set; }
        public string DecumentTags { get; set; }
        public string DecumentTittle { get; set; }
        public string DecumentType { get; set; }

        public string UserType { get; set; }

        public string UserName { get; set; }
        public string CaseName { get; set; }
        public string CaseNumber { get; set; }
        public string DecumentDescripation { get; set; }
        public string DecumentPath { get; set; }
        public string UserId { get; set; }
        public string CaseId { get; set; }
        public string CreatedDate { get; set; }
         public string extention { get; set; }
         public byte[] File { get; set; }
        public int? FirmId { get; set; }
        public int? DirectoryId { get; set; }
        public int? DirectoryLevel { get; set; }
        public string Tokken { get; set; }
        public int? WorkflowId { get; set; }

    }
}
