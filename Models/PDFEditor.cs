using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class PDFEditor
    {
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<PDFEditor> Files { get; set; } = new List<PDFEditor>();
    }
}