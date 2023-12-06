using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Models;
using CapstoneProject.Data;

namespace CapstoneProject.Controllers
{
    [Route("api/prices")]  // Updated route to match the new model name
    [ApiController]
    public class PriceController : Controller  // Renamed the controller to match the new model name
    {
        private readonly ApplicationDbContext _context;

        public PriceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // API endpoint to get all MoviePrices, including related movie and theater data
        [HttpGet("getall")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<MoviePrice>>> GetMoviePrices()  // Updated method name and return type
        {
            var moviePrices = await _context.MoviePrices  // Updated DbSet name
                .Include(mp => mp.Movies)
                .Include(mp => mp.Theaters)
                .ToListAsync();
            return Ok(moviePrices);
        }

        // API endpoint to get a single MoviePrice by ID
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<MoviePrice>> GetMoviePrice(int id)  // Updated method name and return type
        {
            var moviePrice = await _context.MoviePrices  // Updated DbSet name
                .Include(mp => mp.Movies)
                .Include(mp => mp.Theaters)
                .FirstOrDefaultAsync(mp => mp.Id == id);
            if (moviePrice == null)
            {
                return NotFound();
            }
            return Ok(moviePrice);
        }

        // API endpoint to create a MoviePrice
        [HttpPost("create")]
        [Produces("application/json")]
        public async Task<ActionResult<MoviePrice>> Create([FromBody] MoviePrice moviePrice)  // Updated parameter type
        {
            if (ModelState.IsValid)
            {
                _context.MoviePrices.Add(moviePrice);  // Updated DbSet name
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetMoviePrice", new { id = moviePrice.Id }, moviePrice);  // Updated method name
            }
            return BadRequest(ModelState);
        }

        // API endpoint to filter MoviePrices
        [HttpGet("filter")]
        [Produces("application/json")]
        public async Task<IActionResult> FilterMoviePrices(string? MovieTitle, string? TheaterName, DateTime? ShowTime)
        {
            var query = _context.MoviePrices  // Updated DbSet name
                .Include(mp => mp.Movies)
                .Include(mp => mp.Theaters)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(MovieTitle))
            {
                query = query.Where(mp => mp.Movies.Title.Contains(MovieTitle, StringComparison.OrdinalIgnoreCase))
                             .Include(mp => mp.Movies);
            }

            if (!string.IsNullOrWhiteSpace(TheaterName))
            {
                query = query.Where(mp => mp.Theaters.Name.ToLower().Contains(TheaterName.ToLower()))
                             .Include(mp => mp.Theaters);
            }

            if (ShowTime != null)
            {
                query = query.Where(mp => mp.ShowTime.Date == ShowTime.Value.Date);
            }

            var moviePrices = await query.ToListAsync();

            return Ok(moviePrices);
        }

        // API endpoint to edit a MoviePrice
        [HttpPut("edit/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> EditMoviePrice(int id, [FromBody] MoviePrice editedMoviePrice)  // Updated method name and parameter type
        {
            var moviePrice = await _context.MoviePrices.FindAsync(id);  // Updated DbSet name

            if (moviePrice == null)
            {
                return NotFound();
            }

            moviePrice.TicketPrice = editedMoviePrice.TicketPrice;
            moviePrice.ShowTime = editedMoviePrice.ShowTime;

            await _context.SaveChangesAsync();

            return Ok(moviePrice);
        }

        // API endpoint to delete a MoviePrice
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoviePrice(int id)  // Updated method name
        {
            var moviePrice = await _context.MoviePrices.FindAsync(id);  // Updated DbSet name
            if (moviePrice == null)
            {
                return NotFound();
            }
            _context.MoviePrices.Remove(moviePrice);  // Updated DbSet name
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
