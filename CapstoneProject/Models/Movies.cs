using System;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models
{
	public class Movies
	{
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public string Genre { get; set; }

        public string Director { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Display(Name = "Poster Image URL")]
        public string PosterImageUrl { get; set; }


    }
}

