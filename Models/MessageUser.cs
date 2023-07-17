using System.Collections.Generic;

namespace AbsolCase.Models
{
    public class MessageUser
    {
        public string UserId { get; set; }        
        public HashSet<string> ConnectionIds { get; set; }
        public string UserName { get; set; }
    }
}
