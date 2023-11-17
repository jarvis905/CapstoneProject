using System;
namespace CapstoneProject.Models
{
    //This is a model for modeling the data of the www.themoviedb.org

    public class TheMoviedbModel
	{
        public string Title { get; set; }

        public string backdrop_path { get; set; }

        public float vote_average { get; set; }

        public int vote_count { get; set; }

        public string release_date { get; set; }

        public string original_language { get; set; }

        public string overview { get; set; }

        public float popularity { get; set; }

        public bool adult { get; set; }
    }
}

