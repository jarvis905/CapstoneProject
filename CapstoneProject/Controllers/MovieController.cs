using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Data;
using CapstoneProject.Models;

namespace CapstoneProject.Controllers
{
    [ApiController]
    [Route("api/")]
    public class MovieController : Controller
    {

        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            return View();
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
        public async Task<IActionResult> FilterMovies(string? Title, string? Genre, DateTime? ReleaseDate)
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Title))
            {
                query = query.Where(m => EF.Functions.Like(m.Title, $"%{Title}%"));
            }

            if (!string.IsNullOrWhiteSpace(Genre))
            {
                query = query.Where(m => m.Genre == Genre);
            }

            if (ReleaseDate != null)
            {
                query = query.Where(m => m.ReleaseDate == ReleaseDate);
            }

            var movies = await query.ToListAsync();

            return Ok(movies);
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

            if (!string.IsNullOrWhiteSpace(editedMovie.Genre))
            {
                movie.Genre = editedMovie.Genre;
            }

            if (editedMovie.ReleaseDate != null)
            {
                movie.ReleaseDate = editedMovie.ReleaseDate;
            }

            await _context.SaveChangesAsync();

            return Ok(movie);
        }

    }
}

