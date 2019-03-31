using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UpcomingMoviesMobileApp.Shared.Config;
using UpcomingMoviesMobileApp.Shared.Models;

namespace UpcomingMoviesMobileApp.Shared.Services
{
    public class MoviesService
    {
        public async Task<MovieConfig> GetMovies(int category, int page)
        {
            APIConfig api = new APIConfig(category, page);

            MovieConfig returnObject = null;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(api.BaseAdress);
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpResponseMessage response = await client.GetAsync(api.MoviesPath);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string Json = await response.Content.ReadAsStringAsync();
                        returnObject = JsonConvert.DeserializeObject<MovieConfig>(Json);

                        if(returnObject != null)
                        {
                            //Get genres
                            GenreList gList = await GetMoviesGenre(api.BaseAdress, api.GenrePath);

                            if(gList != null)
                            {
                                //Updating movies result with genres
                                foreach (var movie in returnObject.results)
                                {
                                    if(movie.genre_ids != null)
                                    {
                                        movie.genres = gList.genres.Where(g => movie.genre_ids.Any(gId => gId == g.id)).ToList();
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //add Log
            }

            return returnObject;

        }


        public async Task<GenreList> GetMoviesGenre(string baseAdress, string url)
        {

            GenreList returnObject = null;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(baseAdress);
                if (CrossConnectivity.Current.IsConnected)
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string Json = await response.Content.ReadAsStringAsync();
                        returnObject = JsonConvert.DeserializeObject<GenreList>(Json);
                    }
                }

            }
            catch (Exception ex)
            {
                //add Log
            }

            return returnObject;

        }



    }

}
