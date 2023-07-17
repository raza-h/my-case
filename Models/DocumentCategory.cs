using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class DocumentCategory
    {
        [Key]
        [Display(Name = "Category Id")]
        public int DocumentCategoryId { get; set; }
        [Required(ErrorMessage = "Please provide a name for the category")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Display(Name = "Folder")]
        public virtual int? DocuementFolderId { get; set; }
        public virtual int? DocSub3FolderId { get; set; }
        public virtual int? DocSub2FolderId { get; set; }
        public virtual int? DocSub1FolderId { get; set; }
        //public int? DocSubCatId { get; set; }


        [ForeignKey("DocuementFolderId")]
        public  DocumentFolder DocumentFolder { get; set; }
        [ForeignKey("DocSub3FolderId")]
        public  DocSub3Folder DocSub3Folder { get; set; }
        [ForeignKey("DocSub2FolderId")]
        public  DocSub2Folder DocSub2Folder { get; set; }
        [ForeignKey("DocSub1FolderId")]
        public  DocSub1Folder DocSub1Folder { get; set; }
        public string CreatedBy { get; set; }
        //[ForeignKey("DocSub3FolderId")]
        //public  DocSubCategories DocSubCategories { get; set; }

        public int? FirmId { get; set; }

    }



}