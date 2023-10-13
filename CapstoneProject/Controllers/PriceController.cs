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

    [Route("api/prices")]
    [ApiController]

    public class PriceController : Controller
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

        // API endpoint to get all the Price, including related movie and theater data
        [HttpGet("getall")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Price>>> GetPrice()
        {
            var prices = await _context.Price
                .Include(p => p.Movies)
                .Include(p => p.Theaters)
                .ToListAsync();
            return Ok(prices);
        }

        //API endpoint to get a single price by ID
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<Price>> GetPrice(int id)
        {
            var price = await _context.Price
                .Include(p => p.Movies)
                .Include(p => p.Theaters)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (price == null)
            {
                return NotFound();
            }
            return Ok(price);
        }

        // API endpoint to post the Price
        [HttpPost("create")]
        [Produces("application/json")]
        public async Task<ActionResult<Price>> Create([FromBody] Price price)
        {
            if (ModelState.IsValid)
            {
                _context.Price.Add(price);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetPrice", new { id = price.Id }, price);
            }
            return BadRequest(ModelState);
        }

        //API endpoint to filter the Price
        [HttpGet("filter")]
        [Produces("application/json")]
        public async Task<IActionResult> FilterPrices(string? MovieTitle, string? TheaterName, DateTime? ShowTime)
        {
            var query = _context.Price.AsQueryable();

            if (!string.IsNullOrWhiteSpace(MovieTitle))
            {
                query = query.Where(p => p.Movies.Title.Contains(MovieTitle, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(TheaterName))
            {
                query = query.Where(p => p.Theaters.Name.Contains(TheaterName, StringComparison.OrdinalIgnoreCase));
            }

            if (ShowTime != null)
            {
                query = query.Where(p => p.ShowTime.Date == ShowTime.Value.Date);
            }

            var prices = await query.ToListAsync();

            return Ok(prices);
        }

        // API endpoint to edit the Price
        [HttpPut("edit/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> EditPrice(int id, [FromBody] Price editedPrice)
        {
            var price = await _context.Price.FindAsync(id);

            if (price == null)
            {
                return NotFound();
            }

            price.TicketPrice = editedPrice.TicketPrice;

            price.ShowTime = editedPrice.ShowTime;

            await _context.SaveChangesAsync();

            return Ok(price);
        }

        // API endpoint to edit the price
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrice(int id)
        {
            var price = await _context.Price.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }
            _context.Price.Remove(price);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
