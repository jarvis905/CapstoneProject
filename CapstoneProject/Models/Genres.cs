using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace CapstoneProject.Models
{
    public class Genres
    {
        public int Id { get; set; }

        [Display(Name = "Genre")]
        public string GenreName { get; set; }

        [Required]
        [Display(Name = "Movie")]
        public int MovieId { get; set; }


        [ForeignKey("MovieId")]
        public Movies Movie { get; set; }
    }
}
