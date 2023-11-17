using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace CapstoneProject.Models
{
	public class MovieRate
	{
        public int Id { get; set; }

        [Required]
        [Display(Name = "Movie")]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movies Movie { get; set; }
       
        public string Source { get; set; }

        public decimal Value { get; set; }

        [Display(Name = "Number of voters")]
        public  int Voters { get; set; }


    }
}

