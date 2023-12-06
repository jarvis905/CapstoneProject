using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace CapstoneProject.Models
{
	public class MoviePrice
	{
        public int Id { get; set; }

        [Display(Name = "Movies")]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movies Movies { get; set; }

        [Display(Name = "Theatre")]
        public int TheatreId { get; set; }

        [ForeignKey("TheatreId")]
        public Theaters Theaters { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TicketPrice { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Show Time")]
        public DateTime ShowTime { get; set; }


        public int SeatsAvailable { get; set; }

    }
}

