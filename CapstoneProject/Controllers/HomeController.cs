using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CapstoneProject.Models;
using CapstoneProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace CapstoneProject.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly string apiKey = "4612b2e8c775fbf9ab118db655b3536d";
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ApplicationDbContext context , UserManager<IdentityUser> userManager) 
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task<IActionResult> Index()
    {
       
        var MoviesLocal = await _context.Movies.ToListAsync();
        var MoviesApi = await GetPopularMovies();
        var FavMovies = await _context.FavMovies.ToListAsync();

        var model = new MoviesViewModel
        {
            LocalMovies = MoviesLocal,
            PopularMovies = MoviesApi,
            FavMovies = FavMovies
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    private async Task<List<TheMoviedbModel>> GetPopularMovies()
    {
        var client = new HttpClient();

        var apiUrl = $"https://api.themoviedb.org/3/movie/popular?language=en-US&page=1&api_key={apiKey}";

        using (var response = await client.GetAsync(apiUrl))
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var movieResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieResponse>(json);

                if (movieResponse != null)
                {

                    return movieResponse.Results;
                }
            }

            // Handle errors or return an empty list if something went wrong.
            return new List<TheMoviedbModel>();
        }
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToFavorites(int movieId)
    {
        // Get the current user
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized("error unauthorized");
        }

        // Check if the movie is already in the user's favorites
        var existingFavorite = await _context.FavMovies
            .FirstOrDefaultAsync(fm => fm.UserId == user.Id && fm.MovieId == movieId);

        if (existingFavorite != null)
        {
            // Movie is already in favorites, remove it
            _context.FavMovies.Remove(existingFavorite);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Create a new FavoriteMovie record and associate it with the user and movie
        var favoriteMovie = new FavMovies
        {
            UserId = user.Id,
            MovieId = movieId
        };

        _context.FavMovies.Add(favoriteMovie);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}

