using System;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models
{
	public class Price
	{
        public int Id { get; set; }

        [Required]
        [Display(Name = "Movie")]
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movies Movie { get; set; }

        [Display(Name = "Theatre")]
        public int TheatreId { get; set; }
        [ForeignKey("TheatreId")]
        public Theatres Theatre { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Show Time")]
        public DateTime ShowTime { get; set; }

        public int Price { get; set; }

        public int SeatsAvailable { get; set; }
    }
}

