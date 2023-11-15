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

        var popularMovies = await GetMovies("upcoming");
        var nowPlayingMovies = await GetMovies("now_playing");

        var localMovies = await _context.Movies.ToListAsync();
        var favMovies = await _context.FavMovies.ToListAsync();

        var model = new MoviesViewModel
        {
            LocalMovies = localMovies,
            PopularMovies = popularMovies,
            NowPlayingMovies = nowPlayingMovies,
            FavMovies = favMovies
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


    private async Task<List<TheMoviedbModel>> GetMovies(string endpoint)
    {
        var page = 1;

        if (endpoint == "now_playing")
        {
            // Set the page to 2 only for "now_playing" endpoint
            page = 2;
        }

        var client = new HttpClient();
        var apiUrl = $"https://api.themoviedb.org/3/movie/{endpoint}?language=en-US&page={page}&api_key={apiKey}";

        using (var response = await client.GetAsync(apiUrl))
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (endpoint == "upcoming" || endpoint == "now_playing")
                {
                    var movieResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieResponse<TheMoviedbModel>>(json);
                    return movieResponse?.Results ?? new List<TheMoviedbModel>();
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

