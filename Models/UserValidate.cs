using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsolCase.Models
{
    public class UserValidate
    {
        [NotMapped]
        public string CurrentUserEmail { get; set; }
    }
    [MetadataType(typeof(UserValidate))]
    public partial class AspNetUsers
    {
        [NotMapped]
        public string CurrentUserEmail { get; set; }
    }

}
