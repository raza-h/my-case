using System.ComponentModel.DataAnnotations.Schema;

namespace AbsolCase.Models
{
    public class CFieldValue
    {
        public int FieldValueID { get; set; }
        public int FieldID { get; set; }
        public string ModuleType { get; set; }
        public int ConcernID { get; set; }
        public string Value { get; set; }
        [NotMapped]
        public string PracticeAreaName { get; set; }
    }
}
