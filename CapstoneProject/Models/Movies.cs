using System;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models
{
	public class Movies
	{
        public int Id { get; set; }

        public int MovieID { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public string? Director { get; set; }

        public string? Writer { get; set; }

        public string? Actores { get; set; }

        public string Language { get; set; }

        public string? Country { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Display(Name = "Poster Image URL")]
        public string PosterImageUrl { get; set; }

        public string? RunTime { get; set; }

        [Display(Name = "Age Rating")]
        public string? MovieRating { get; set; }

        public long? BoxOffice { get; set; }

        public string? TrailerKey { get; set; }
    }
}

