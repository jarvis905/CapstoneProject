using System;
namespace CapstoneProject.Models
{
	public class MoviesViewModel
	{
        //Get list of movies from the local DB
        public List<Movies> LocalMovies { get; set; }

        //Get list of popular movies from API
        public List<TheMoviedbModel> PopularMovies { get; set; }
    }
}

