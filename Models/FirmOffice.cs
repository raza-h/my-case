using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class FirmOffice
    {
        public int Id { get; set; }
        public int FirmId { get; set; }
        public string OfficeName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPrimary { get; set; }
        public List<FirmOfficeAddress> firmOfficeAddresses { get; set; }
    }
}
