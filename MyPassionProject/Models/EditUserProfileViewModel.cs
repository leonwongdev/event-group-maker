using System.ComponentModel.DataAnnotations;

namespace MyPassionProject.Models
{
    public class EditUserProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string FullName { get; set; }

        [Required]
        // Composite formatting: https://learn.microsoft.com/en-us/dotnet/standard/base-types/composite-formatting
        [StringLength(255, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Bio { get; set; }


        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string LinkedinUrl { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string GithubUrl { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string PortfolioUrl { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Role { get; set; }
    }
}