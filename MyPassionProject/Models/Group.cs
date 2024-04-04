using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPassionProject.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        // Foreign key reference to Hackathon, the name has to be same as the name of nvigation property below
        [ForeignKey("Hackathon")]
        public int HackathonId { get; set; }

        // Navigation property
        public virtual Hackathon Hackathon { get; set; }
        public string TeamLeaderId { get; set; }

        [StringLength(300, MinimumLength = 10, ErrorMessage = "Requirements must be between 10 and 300 characters.")]
        public string Requirements { get; set; }

        [Display(Name = "Max Number Of Members")]
        [Range(2, int.MaxValue, ErrorMessage = "Value must be 2 or greater")]
        public int MaxNumOfMembers { get; set; }

        // Navigation property for Event 
        public int EventId { get; set; }
        public Event Event { get; set; }

    }
}