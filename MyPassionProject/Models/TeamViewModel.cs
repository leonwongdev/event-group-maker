using System.Collections.Generic;

namespace MyPassionProject.Models
{
    public class TeamViewModel
    {
        public List<TeamDTO> TeamDTO { get; set; }

        public Hackathon Hackathon { get; set; } // current hackathon

        public ApplicationUser CurrentUser { get; set; } // current user
    }
}