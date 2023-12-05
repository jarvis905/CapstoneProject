using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CapstoneProject.Models
{
	public class MovieViewModel
	{
        public Movies MovieData {get; set; }

        public List<Reviews> ListReviews {get; set; }

        public UserManager<IdentityUser> UserManager {get; set; }
    }
}

