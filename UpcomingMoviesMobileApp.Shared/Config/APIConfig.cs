using UpcomingMoviesMobileApp.Shared.Models;

namespace UpcomingMoviesMobileApp.Shared.Config
{
    public class APIConfig
    {
        public string BaseAdress { get; set; }
        public string ContentType { get; set; }
        public string ApiKey { get; set; }
        public string MoviesPath { get; set; }
        public string GenrePath { get; set; }


        //private string genrePath = "/genre/movie/list?";
        //private string ApiKey = "1f54bd990f1cdfb230adb312546d765d";

        public APIConfig()
        {
            BaseAdress = "https://api.themoviedb.org/3/";
            ApiKey = "1f54bd990f1cdfb230adb312546d765d";
            ContentType = "application/json";
        }

        public APIConfig(int category, int page)
        {
            BaseAdress = "https://api.themoviedb.org/3/";
            ApiKey = "1f54bd990f1cdfb230adb312546d765d";
            MoviesPath = GetParamsMovie(category) + "api_key=" + ApiKey + "&page=" + page;
            GenrePath = "genre/movie/list?api_key=" + ApiKey;
            ContentType = "application/json";
        }

       
        public string GetParamsMovie(int category)
        {
            switch (category)
            {
                case (int)MovieCategory.Popular:
                    return "movie/popular?";

                case (int)MovieCategory.TopRated:
                    return "movie/top_rated?";

                case (int)MovieCategory.Upcoming:
                    return "movie/upcoming?";

                case (int)MovieCategory.NowPlaying:
                    return "movie/now_playing?";

                default:
                    return string.Empty;

            }
        }


    }
}
