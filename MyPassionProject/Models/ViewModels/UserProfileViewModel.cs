using System.Collections.Generic;

namespace MyPassionProject.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public List<ApplicationUserGroup> ApplicationUserGroups { get; set; }

    }
}