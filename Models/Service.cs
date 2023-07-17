using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter Service")]
        public string Name { get; set; }
    }
}
