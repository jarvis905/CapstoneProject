using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Data;
using CapstoneProject.Models;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using CapstoneProject.Controllers;
using System.Diagnostics;

namespace CapstoneProject.Controllers
{
    [ApiController]
    [Route("api/")]
    public class MovieController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MovieController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var model = new MoviesViewModel
            {
                LocalMovies = await _context.Movies.ToListAsync(),
            };
            return View(model);
        }

        [HttpGet("movie/{id}")]
        public async Task<IActionResult> SingleMovie(int id)
        {
            
            var MovieData = await _context.Movies.FirstOrDefaultAsync(r => r.Id == id);
            var ReviewList = await _context.Reviews.Where(r => r.MovieId == id).ToListAsync();
            

            if (MovieData == null)
            {
                return NotFound();
            }

            var model = new MovieViewModel
            {
                MovieData = MovieData,
                ListReviews = ReviewList,
                UserManager = _userManager
            };

            return View(model);
        }


        // API endpoint to get all the Movies
        [HttpGet("getall")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Movies>>> GetAll()
        {
            return await _context.Movies.ToListAsync();
        }

        // API endpoint to post the Movie
        [HttpPost("create")]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] Movies movies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movies);
                await _context.SaveChangesAsync();
                return Ok(movies);
            }
            return BadRequest(ModelState);
        }

        // API endpoint to filter the Movies
        [HttpGet("filter")]
        [Produces("application/json")]
        public async Task<IActionResult> FilterMovies(string? searchString)
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(m => m.Title.ToLower().Contains(searchString.ToLower()));
            }

            var model = new MoviesViewModel
            {
                LocalMovies = await query.ToListAsync(),
            };

            return View("Index", model);
        }

        // API endpoint to edit the Movie
        //We can use HttpPost instead of HttpPut as well
        [HttpPut("edit/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> EditMovie(int id, [FromBody] Movies editedMovie)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                // Return 404 if the movie with the given ID is not found
                return NotFound();
            }

            // Update the movie properties based on the provided editedMovie
            if (!string.IsNullOrWhiteSpace(editedMovie.Title))
            {
                movie.Title = editedMovie.Title;
            }

            if (editedMovie.ReleaseDate != null)
            {
                movie.ReleaseDate = editedMovie.ReleaseDate;
            }

            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        //Delete movie
        [HttpDelete("delete/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                // Return 404 if the movie with the given ID is not found
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return Ok("Movie deleted successfully");
        }



        [HttpGet("random")]
        [Produces("application/json")]
        public async Task<IActionResult> RandomMovie()
        {
            var randomMovieId = await FetchRandomMovieIdFromDatabase();

            if (randomMovieId == null)
            {
                // Return a 404 status code if no movie is found
                return NotFound(); 
            }

            var randomMovie = await _context.Movies.FindAsync(randomMovieId);

            if (randomMovie == null)
            {
                // Return a 404 status code if the movie is not found in the database
                return NotFound(); 
            }
            ViewBag.RandomMovieImageUrl = randomMovie.PosterImageUrl;
            //return View();
            return Ok(randomMovie.PosterImageUrl);
        }

        // Helper method to fetch a random movie ID from the database
        private async Task<int?> FetchRandomMovieIdFromDatabase()
        {
            var totalMovies = await _context.Movies.CountAsync();

            if (totalMovies == 0)
            {
                // No movies in the database
                return null; 
            }

            var random = new Random();
            // Generate a random movie ID between 1 and totalMovies
            var randomIndex = random.Next(1, totalMovies + 1); 

            return randomIndex;
        }

    }
}

