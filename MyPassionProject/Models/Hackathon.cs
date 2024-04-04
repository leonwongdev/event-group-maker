using System;
using System.ComponentModel.DataAnnotations;

namespace MyPassionProject.Models
{
    public class Hackathon
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, ErrorMessage = "Name must be at most 255 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(255, ErrorMessage = "Location must be at most 255 characters.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(300, ErrorMessage = "Description must be at most 300 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "URL is required.")]
        [StringLength(255, ErrorMessage = "URL must be at most 255 characters.")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Deadline is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid deadline format.")]
        public DateTime Deadline { get; set; }

    }
}