using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Data;
using CapstoneProject.Models;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Controllers
{
    [ApiController]
    [Route("api/theaters")]
    public class TheaterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TheaterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index(string? city = null, string? search = null, DateTime? selectedDate = null)
        {
            IQueryable<Theaters> query = _context.Theaters.AsQueryable();

            // Apply city filter if specified
            if (!string.IsNullOrWhiteSpace(city) && city != "All Cities")
            {
                query = query.Where(t => t.City == city);
            }

            // Apply search filter if specified
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => EF.Functions.Like(t.Name, $"%{search}%"));
            }

            var theaters = await query.ToListAsync();

            // Convert the list of theaters to TheaterWithPricesViewModel
            var theatersWithPrices = new List<TheaterWithPricesViewModel>();

            foreach (var theater in theaters)
            {
                var moviePrices = await _context.MoviePrices
                    .Include(p => p.Movies)
                    .Include(p => p.Theaters)
                    .Where(p => p.Theaters.Id == theater.Id)
                    .ToListAsync();

                // Apply date filter if specified
                if (selectedDate.HasValue)
                {
                    moviePrices = moviePrices
                        .Where(p => p.ShowTime.Date == selectedDate.Value.Date)
                        .ToList();
                }

                // Create a view model for the theater with associated prices
                var theaterWithPrices = new TheaterWithPricesViewModel
                {
                    Theater = theater,
                    Prices = moviePrices
                };

                theatersWithPrices.Add(theaterWithPrices);
            }

            return View(theatersWithPrices);
        }






        // Endpoint to get all theaters
        [HttpGet("getall")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Theaters>>> GetAll()
        {
            return await _context.Theaters.ToListAsync();
        }

        // Endpoint to create a new theater
        [HttpPost("create")]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] Theaters theater)
        {
            if (ModelState.IsValid)
            {
                _context.Add(theater);
                await _context.SaveChangesAsync();
                return Ok(theater);
            }
            return BadRequest(ModelState);
        }

        // Endpoint to filter theaters based on criteria
        [HttpGet("filter")]
        [Produces("application/json")]
        public async Task<IActionResult> FilterTheaters(
            string? Name, string? Location, DateTime? OpeningDate, string? City)
        {
            var query = _context.Theaters.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Name))
            {
                query = query.Where(t => EF.Functions.Like(t.Name, $"%{Name}%"));
            }

            if (!string.IsNullOrWhiteSpace(Location))
            {
                query = query.Where(t => t.Location == Location);
            }

            if (OpeningDate != null)
            {
                query = query.Where(t => t.OpeningDate == OpeningDate);
            }

            if (!string.IsNullOrWhiteSpace(City) && City != "All Cities")
            {
                query = query.Where(t => t.City == City);
            }

            var theaters = await query.ToListAsync();

            return View("Index", theaters); //Ok(theaters);
        }

        // Endpoint to edit a theater
        [HttpPut("edit/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> EditTheater(int id, [FromBody] Theaters editedTheater)
        {
            var theater = await _context.Theaters.FindAsync(id);

            if (theater == null)
            {
                // Return 404 if the theater with the given ID is not found
                return NotFound();
            }

            // Update theater properties based on the provided editedTheater
            if (!string.IsNullOrWhiteSpace(editedTheater.Name))
            {
                theater.Name = editedTheater.Name;
            }

            if (!string.IsNullOrWhiteSpace(editedTheater.Location))
            {
                theater.Location = editedTheater.Location;
            }

            if (editedTheater.OpeningDate != null)
            {
                theater.OpeningDate = editedTheater.OpeningDate;
            }

            if (!string.IsNullOrWhiteSpace(editedTheater.Description))
            {
                theater.Description = editedTheater.Description;
            }

            if (editedTheater.Capacity > 0)
            {
                theater.Capacity = editedTheater.Capacity;
            }

            if (!string.IsNullOrWhiteSpace(editedTheater.Address))
            {
                theater.Address = editedTheater.Address;
            }

            if (!string.IsNullOrWhiteSpace(editedTheater.ContactInformation))
            {
                theater.ContactInformation = editedTheater.ContactInformation;
            }

            if (!string.IsNullOrWhiteSpace(editedTheater.City))
            {
                theater.City = editedTheater.City;
            }

            if (!string.IsNullOrWhiteSpace(editedTheater.CinemaLine))
            {
                theater.CinemaLine = editedTheater.CinemaLine;
            }

            // Similar updates for other fields

            await _context.SaveChangesAsync();

            return Ok(theater);
        }

        // Endpoint to get all theaters with associated movie prices
        [HttpGet("getallwithprices")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<TheaterWithPricesViewModel>>> GetAllWithPrices()
        {
            var theaters = await _context.Theaters.ToListAsync();

            // Create a list to hold the view model with prices
            var theatersWithPrices = new List<TheaterWithPricesViewModel>();

            foreach (var theater in theaters)
            {
                var moviePrices = await _context.MoviePrices
                    .Include(p => p.Movies)
                    .Include(p => p.Theaters)  // Include the Theaters navigation property
                    .Where(p => p.Theaters.Id == theater.Id)  
                    .ToListAsync();

                // Create a view model for the theater with associated prices
                var theaterWithPrices = new TheaterWithPricesViewModel
                {
                    Theater = theater,
                    Prices = moviePrices
                };

                theatersWithPrices.Add(theaterWithPrices);
            }

            return theatersWithPrices;
        }

    }
}
