using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Entities
{
    public class UserAgainstCase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? CaseId { get; set; }
        public string BillingType { get; set; }
        public string Rate { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string Email { get; set; }
    }
}
