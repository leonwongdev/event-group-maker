using System.Collections.Generic;

namespace MyPassionProject.Models.ViewModels
{
    public class FindAppUser
    {
        //This is a class
        //public AppUserDto SelectedAppUser { get; set; }

        public IEnumerable<EventDto> PaticipatingEvents { get; set; }
    }
}