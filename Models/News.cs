using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class News
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "News Title Required")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string NewsTittle { get; set; }
        [Required(ErrorMessage = "News Description Required")]
        [RegularExpression(@"^[^\s]+(\s+[^\s]+)*$", ErrorMessage = "Please avoid white spaces.")]
        public string NewsDescription { get; set; }
        public string NewsType { get; set; }
        public string SendTo { get; set; }
        public DateTime? Time { get; set; }
        public bool status { get; set; }
        [Required(ErrorMessage = "News ExpireDate Required")]
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpireDate { get; set; }
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "News PublishDate Required")]
        public DateTime? PublishDate { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int? FirmId { get; set; }

    }
}
