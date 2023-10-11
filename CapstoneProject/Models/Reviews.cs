using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CapstoneProject.Models
{
    public class Reviews
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Movie")]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movies Movie { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; } // Using a custom ApplicationUser if needed later

        [Range(1, 5, ErrorMessage = "The rating must be between 1 and 10.")]
        public int Rating { get; set; }

        [MaxLength(500)] 
        public string Comment { get; set; }

        [Display(Name = "Timestamp")]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }
    }
}
