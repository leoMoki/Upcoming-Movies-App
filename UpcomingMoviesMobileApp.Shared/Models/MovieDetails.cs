using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMoviesMobileApp.Shared.Models
{
    public class MovieDetails
    {
        public string Title { get; set; }
        public List<Genre> Genres { get; set; }
        public string ReleaseDate { get; set; }
        public string Overview { get; set; }
        public string ImageUrl { get; set; }
    }
}
