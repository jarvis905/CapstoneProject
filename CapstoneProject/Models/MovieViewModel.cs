using System;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Models
{
	public class MovieViewModel
	{
        public List<Movies> MovieData {get; set; }

        public List<Reviews> ListReviews {get; set; }
    }
}

