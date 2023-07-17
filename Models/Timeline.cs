using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public enum TimeLineType
    {
        Comment=1,
        Document=2,
        Message=3,
        Zoom=4
    }
    public class Timeline
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? CaseId { get; set; }
        public string Comment { get; set; }
        public string FilePath { get; set; }
        public string UserImagePath { get; set; }
        public string UserName { get; set; }
        public string CaseName { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ClientId { get; set; }
        public TimeLineType TimeLineType { get; set; }

        public string HostLink { get; set; }
        public string JoinLink { get; set; }
        [NotMapped]
        public string duration { get; set; }
        [NotMapped]
        public string topic { get; set; }
        [NotMapped]
        public string starttime { get; set; }
        [NotMapped]
        public string timezone { get; set; }
        [NotMapped]
        public string FileType { get; set; }
        public Byte[] VideoFilePathbyte { get; set; }
        public string VideoFilePath { get; set; }
        public string DocFilePath { get; set; }
        public Byte[] DocFilePathbyte { get; set; }
    }
}
