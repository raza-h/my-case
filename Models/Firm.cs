using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class Firm
    {
        public int Id { get; set; }

        public string FirmName { get; set; }
      
        public string FirmNumber { get; set; }
       
        public string FirmEmail { get; set; }
        public string RegistrationNumber { get; set; }
        public string OwnerName { get; set; }
        public int NumberofEmployees { get; set; }
        public string FirmWebsite { get; set; }
        public string UserId { get; set; }
        public List<FirmOffice> FirmOffices { get; set; }
    }
}
