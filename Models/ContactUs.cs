using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class ContactUs
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Enter Your Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Your Name")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Your Number")]
       
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter Your Address")]
        public string Address { get; set; }

        public string  Comment { get; set; }



    }
}
