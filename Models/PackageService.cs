using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase.Models
{
    public class PackageService
    {
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public int? PricePlanId { get; set; }
        public List<Service> services { get; set; }
        public PricePlan pricePlan { get; set; }
        public int[] ServicesIds { get; set; }
    }
}
