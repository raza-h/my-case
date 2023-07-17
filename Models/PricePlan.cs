using System;
using System.ComponentModel.DataAnnotations;

namespace AbsolCase.Models
{
    public class PricePlan
    {
        public int PlanID { get; set; }
        [Required(ErrorMessage = "Please Enter Plan Name")]
        [RegularExpression(@"^.{3,}$", ErrorMessage = "Minimum 3 characters required")]
        public string PlanName { get; set; }
        [Required(ErrorMessage = "Please Enter Price")]
        [RegularExpression(@"^(?=.*[1-9])(?:[1-9]\d*\.?|0?\.)\d*$", ErrorMessage = "Please Enter Valid Price.")]
        public string PriceRange { get; set; }
        [CannotBeEmptyAttribute(ErrorMessage ="Please select atleast one service")]
        public int[] ServicesIds { get; set; }
        public string TimeRange { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
