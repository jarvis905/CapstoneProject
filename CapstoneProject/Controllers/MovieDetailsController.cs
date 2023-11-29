using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneProject.Data;
using CapstoneProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CapstoneProject.Controllers
{
    [ApiController]
    [Route("api/details")]
    public class MovieDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly string _apiKey = "4612b2e8c775fbf9ab118db655b3536d";


        public MovieDetailsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? movieId)
        {

            if (movieId == null)
            {
                // Handle the case where movieId is null
                return NotFound();
            }

            // Retrieve the movie details from the local database based on the movieId
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == movieId);

            if (movie == null)
            {
                // Handle the case where the movie is not found
                return NotFound();
            }

            // Retrieve reviews for the specified movie
            var movieReviews = await _context.Reviews
                .Where(r => r.MovieId == movie.Id)
                .ToListAsync();

            // Fetch movie trailers from the API
            var trailers = await GetMovieTrailersAsync(movieId);

            // Create a view model for the movie details and reviews
            var movieViewModel = new MoviesViewModel
            {
                MovieDetails = movie,
                MovieReviews = movieReviews,
                MovieTrailers = trailers, 
                UserManager = _userManager
            };

            return View(movieViewModel);  
        }

        private async Task<List<MovieTrailer>> GetMovieTrailersAsync(int? movieId)
        {
            if (movieId == null)
            {
                return new List<MovieTrailer>();
            }

            // Construct the API URL for fetching movie trailers
            string apiUrl = $"https://api.themoviedb.org/3/movie/{movieId}/videos?api_key={_apiKey}";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetStringAsync(apiUrl);

                    // Deserialize the response to get the trailer key
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieResponse<MovieTrailer>>(response);

                    if (result?.Results?.Count > 0)
                    {
                        return result.Results.Select(trailer => new MovieTrailer
                        {
                            Key = trailer.Key,
                            id = trailer.id,
                            name = trailer.name 
                        }).ToList();
                    }
                }
                catch (HttpRequestException)
                {
                    // Handle exception if API request fails
                }
            }

            return new List<MovieTrailer>();
        }


    }
}

