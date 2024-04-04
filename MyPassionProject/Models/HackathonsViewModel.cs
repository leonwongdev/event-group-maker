using System.Collections.Generic;

namespace MyPassionProject.Models
{
    public class HackathonsViewModel
    {
        public List<Hackathon> Hackathons { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }
}