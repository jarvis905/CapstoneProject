using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<CapstoneProject.Models.Movies> Movies { get; set; }
    public DbSet<CapstoneProject.Models.Price> Price { get; set; }
    public DbSet<CapstoneProject.Models.Reviews> Reviews { get; set; }
    public DbSet<CapstoneProject.Models.Theaters> Theaters { get; set; }
    public DbSet<CapstoneProject.Models.Genres> Genres { get; set; }
    public DbSet<CapstoneProject.Models.MovieRate> MovieRates { get; set; }
    public DbSet<CapstoneProject.Models.FavMovies> FavMovies { get; set; }
    public DbSet<CapstoneProject.Models.MoviePrice> MoviePrices { get; set; }


}

