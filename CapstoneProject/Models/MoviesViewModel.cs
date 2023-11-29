using System;
using Microsoft.AspNetCore.Identity;

namespace CapstoneProject.Models
{
	public class MoviesViewModel
	{
        //Get list of movies from the local DB
        public List<Movies> LocalMovies { get; set; }

        //Get list of popular movies from API
        public List<TheMoviedbModel> PopularMovies { get; set; }

        public List<FavMovies> FavMovies { get; set; }

        // Get list of now playing movies from API
        public List<TheMoviedbModel> NowPlayingMovies { get; set; }

        public Movies MovieDetails { get; set; }

        public List<Reviews> MovieReviews { get; set; }

        public Reviews PostReview { get; set; }

        public UserManager<IdentityUser> UserManager { get; set; }

        public List<MovieTrailer> MovieTrailers { get; set; }
    }
}

