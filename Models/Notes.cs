using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class Notes
    {
        [Key]
        public int Id { get; set; }
        public string NotesTag { get; set; }
        public string NotesTittle { get; set; }
        public string NotesType { get; set; }
        public string NotesDescripation { get; set; }
        public string UserId { get; set; }
        public string CaseId { get; set; }
        public string CreatedDate { get; set; }
        [NotMapped]
        public string CaseName { get; set; }
        public int? FirmId { get; set; }
        [NotMapped]
        public int OldCaseId { get; set; }


    }
}
