using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPassionProject.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        // Foreign key reference to Hackathon, the name has to be same as the name of nvigation property below
        [ForeignKey("Event")]
        public int EventId { get; set; }

        // Navigation property
        [JsonIgnore]
        public virtual Event Event { get; set; }
        public string TeamLeaderId { get; set; }

        [StringLength(300, MinimumLength = 10, ErrorMessage = "Requirements must be between 10 and 300 characters.")]
        public string Requirements { get; set; }

        [Display(Name = "Max Number Of Members")]
        [Range(2, int.MaxValue, ErrorMessage = "Value must be 2 or greater")]
        public int MaxNumOfMembers { get; set; }
    }
}