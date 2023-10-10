using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CapstoneProject.Models
{
    public class Theaters
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Theater Name")]
        public string Name { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Display(Name = "Opening Date")]
        [DataType(DataType.Date)]
        public DateTime OpeningDate { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Contact Information")]
        public string ContactInformation { get; set; }
    }
}
