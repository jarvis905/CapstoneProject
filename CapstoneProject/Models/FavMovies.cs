using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace CapstoneProject.Models
{
	public class FavMovies
	{
        public int Id { get; set; }

        // Foreign Key for User
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        // Foreign Key for Movie
        public int MovieId { get; set; }
        public Movies Movie { get; set; }

        
    }
}

