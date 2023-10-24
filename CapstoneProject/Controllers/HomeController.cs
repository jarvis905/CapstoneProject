using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CapstoneProject.Models;
using CapstoneProject.Data;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    

    public HomeController(ApplicationDbContext context) 
    {
        _context = context;
    }


    public async Task<IActionResult> Index()
    {
        var movies = await _context.Movies.ToListAsync();
        return View(movies);
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
}

