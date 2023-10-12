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
            string? Name, string? Location, DateTime? OpeningDate)
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

            var theaters = await query.ToListAsync();

            return Ok(theaters);
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

            // Similar updates for other fields

            await _context.SaveChangesAsync();

            return Ok(theater);
        }
    }
}
