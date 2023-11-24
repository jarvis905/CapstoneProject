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

        // Retrieve popular and now playing movies from external APIs
        var popularMovies = await GetMovies("upcoming");
        var nowPlayingMovies = await GetMovies("now_playing");

        // Get the list of local movies from the database
        var localMovies = await _context.Movies.ToListAsync();

        // Check and add new popular movies to the local database
        foreach (var popularMovie in popularMovies)
        {
            if (!localMovies.Any(m => m.MovieID == popularMovie.id))
            {
                _context.Movies.Add(new Movies
                {
                    // Map the properties from popularMovie to the Movies entity
                    MovieID = popularMovie.id,
                    Title = popularMovie.Title,
                    ReleaseDate = DateTime.Parse(popularMovie.release_date),
                    Description = popularMovie.overview,
                    PosterImageUrl = "https://www.themoviedb.org/t/p/w1280" + popularMovie.backdrop_path,
                    Language = popularMovie.original_language
                });
            }
        }

        // Check and add new now playing movies to the local database
        foreach (var nowPlayingMovie in nowPlayingMovies)
        {
            if (!localMovies.Any(m => m.MovieID == nowPlayingMovie.id))
            {
                _context.Movies.Add(new Movies
                {
                    // Map the properties from nowPlayingMovie to the Movies entity
                    MovieID = nowPlayingMovie.id,
                    Title = nowPlayingMovie.Title,
                    ReleaseDate = DateTime.Parse(nowPlayingMovie.release_date),
                    Description = nowPlayingMovie.overview,
                    PosterImageUrl = "https://www.themoviedb.org/t/p/w1280" + nowPlayingMovie.backdrop_path,
                    Language = nowPlayingMovie.original_language
                });
            }
        }

        // Save changes to the local database
        await _context.SaveChangesAsync();

        // Retrieve the latest list of local movies
        localMovies = await _context.Movies.ToListAsync();

        // Retrieve favorite movies from the local database
        var favMovies = await _context.FavMovies.ToListAsync();

        // Create the view model
        var model = new MoviesViewModel
        {
            LocalMovies = localMovies,
            PopularMovies = popularMovies,
            NowPlayingMovies = nowPlayingMovies,
            FavMovies = favMovies
        };

        // Pass the view model to the view
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


    public async Task<IActionResult> MovieDetails(int movieId)
    {
        // Retrieve the movie details from the local database based on the movieId
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == movieId);

        if (movie == null)
        {
            // Handle the case where the movie is not found
            return NotFound();
        }

        // Create a view model for the movie details
        var MovieDetails = new MoviesViewModel
        {
            MovieDetails = movie,
            
        };

        return View(movie);
    }
}

