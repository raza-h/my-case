using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class DocSub3Folder
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual int DocSub2FolderId { get; set; }
        [ForeignKey("DocSub2FolderId")]
        public  DocSub2Folder DocSub2Folder { get; set; }
        
        [NotMapped]
        public List<string> DocSubFolders { get; set; }
        public ICollection<DocumentCategory> DocumentCategory { get; set; }
        public int? FirmId { get; set; }

    }
}