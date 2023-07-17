using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbsolCase
{
    public class ArchiveContact
    {
        public int Id { get; set; }
        // for current logged in user
        public string ContactOne { get; set; }
        // for other user we have chat with
        public string ContactTwo { get; set; }
        public int? GroupId { get; set; }
    }
}
