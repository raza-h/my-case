using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class DocSub2Folder
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual int DocSub1FolderId { get; set; }
        [ForeignKey("DocSub1FolderId")]
        public  DocSub1Folder DocSub1Folder { get; set; }
        public ICollection<DocSub3Folder> DocSub3Folders { get; set; }

        public ICollection<DocumentCategory> DocumentCategory { get; set; }
        public int? FirmId { get; set; }
    }
}