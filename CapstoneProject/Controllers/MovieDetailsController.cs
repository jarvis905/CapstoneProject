using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneProject.Data;
using CapstoneProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace CapstoneProject.Controllers
{
    [ApiController]
    [Route("api/details")]
    public class MovieDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? movieId)
        {
            // Retrieve the movie details from the local database based on the movieId
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == movieId);

            if (movie == null)
            {
                // Handle the case where the movie is not found
                return NotFound();
            }

            // Create a view model for the movie details and reviews
            var movieViewModel = new MoviesViewModel
            {
                MovieDetails = movie,
               
            };

            return View(movieViewModel);  
        }

    }
}

