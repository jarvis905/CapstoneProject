using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Data;
using CapstoneProject.Models;
using Microsoft.AspNetCore.Identity;


namespace CapstoneProject.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reviews
        public IActionResult Index()
        {
            return View();
        }

        // Get: getall
        [HttpGet("getall")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Reviews>>> GetAll()
        {
            var reviews = await _context.Reviews
            .Include(r => r.Movie)
            .Include(r => r.User)
            .Select(r => new
            {
                r.Id,
                r.MovieId,
                MovieTitle = r.Movie.Title,  // Select only the title of the related movie
                r.UserId,
                UserName = r.User.UserName,  // Select only the username of the related user
                r.Rating,
                r.Comment,
                r.Timestamp
            })
            .ToListAsync();

            return Ok(reviews);
        }

        // API endpoint to get reviews of a specific movie by ID
        [HttpGet("movie/{movieid}")]
        [Produces("application/json")]
        public async Task<ActionResult<Reviews>> GetReviews(int movieid)
        {
            var reviews = from r in _context.Reviews select r;
            reviews = reviews.Where(r => r.MovieId == movieid);
            if (reviews == null)
            {
                return NotFound();
            }
            return Ok(await reviews.ToListAsync());
        }

        // API endpoint to get reviews of a specific User by ID
        [HttpGet("user/{userid}")]
        [Produces("application/json")]
        public async Task<ActionResult<Reviews>> GetReviewsByUser(String userid)
        {
            var reviews = from r in _context.Reviews select r;
            reviews = reviews.Where(r => r.UserId == userid);
            if (reviews == null)
            {
                return NotFound();
            }
            return Ok(await reviews.ToListAsync());
        }

        // API endpoint to post a review for a movie
        [HttpPost("write")]
        [Produces("application/json")]
        public async Task<ActionResult<Reviews>> WriteReview([FromBody] Reviews review)
        {
            if (ModelState.IsValid)
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetReviews", new { id = review.Id }, review);
            }
            return BadRequest(ModelState);
        }

        // API endpoint to edit a review
        [HttpPut("edit/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> EditReview(int id, [FromBody] Reviews editedReview)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            review.Comment = editedReview.Comment;

            review.Rating = review.Rating;

            await _context.SaveChangesAsync();

            return Ok(review);
        }

        // API endpoint to delete a review
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
