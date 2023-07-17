using System;
using System.ComponentModel.DataAnnotations;

namespace AbsolCase.Models
{
    public class ClientLocation
    {
        [Key]
        public int ClientLocationId { get; set; }
        public int FirmId { get; set; }
        public string Longitude { get; set; }
        public string Lattitude { get; set; }
        public string PlaceName { get; set; }
        public string CityName { get; set; }
        public DateTime Datetime { get; set; }
        public int CaseId { get; set; }
        public string UserId { get; set; }
        public string DeviceIP { get; set; }
        public string DeviceDNS { get; set; }
    }
}
