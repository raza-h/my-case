using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class FormAdd
    {
        [Key]
        public int Id { get; set; }
        public string NotesTag { get; set; }
        public string NotesTittle { get; set; }


    }
}
