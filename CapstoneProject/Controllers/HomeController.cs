using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CapstoneProject.Models;
using CapstoneProject.Data;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly string apiKey = "4612b2e8c775fbf9ab118db655b3536d";

    public HomeController(ApplicationDbContext context) 
    {
        _context = context;
    }


    public async Task<IActionResult> Index()
    {
        var MoviesLocal = await _context.Movies.ToListAsync();
        var MoviesApi = await GetPopularMovies();

        var model = new MoviesViewModel
        {
            LocalMovies = MoviesLocal,
            PopularMovies = MoviesApi
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

}

