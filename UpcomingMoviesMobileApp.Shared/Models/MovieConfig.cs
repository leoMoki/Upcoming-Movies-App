﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UpcomingMoviesMobileApp.Shared.Models
{
    public class MovieConfig
    {
        public int page { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
        public List<Movie> results { get; set; }
    }
}
