using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPassionProject.Models
{
    /// <summary>
    /// Junction Class / Assocation class of between Team and Application User
    /// </summary>
    public class ApplicationUserGroup
    {
        // Using composite Key here to prevent one user from joing multiple teams of the same hackathon
        [Key, Column(Order = 0), ForeignKey("User")]
        public string UserId { get; set; }

        [Column(Order = 1), ForeignKey("Group")]
        public int GroupId { get; set; }

        [Key, Column(Order = 2), ForeignKey("Hackathon")]
        public int HackathonId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }
        public virtual Hackathon Hackathon { get; set; }
    }
}