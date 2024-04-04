using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models
{
    public class TeamDTO
    {
        public Group Team { get; set; }
        public ApplicationUser TeamLeader {  get; set; }
    }
}