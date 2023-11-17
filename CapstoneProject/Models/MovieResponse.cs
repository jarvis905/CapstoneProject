using System;
namespace CapstoneProject.Models
{
    public class MovieResponse<T>
    {
        public List<T> Results { get; set; }
    }
}

