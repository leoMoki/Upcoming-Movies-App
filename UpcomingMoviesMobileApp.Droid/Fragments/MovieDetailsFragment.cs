using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Com.Squareup.Picasso;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using UpcomingMoviesMobileApp.Shared.Models;

namespace UpcomingMoviesMobileApp.Droid.Fragments
{
    public class MovieDetailsFragment : Fragment
    {
        TextView tvTitle, tvGenre, tvReleaseDate, tvOverview;
        ImageView ivMovieDetail;
        MovieDetails movieDetails;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Arguments != null)
            {
                 movieDetails = JsonConvert.DeserializeObject<MovieDetails>(Arguments.GetString("MovieDetail"));
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.movie_details_fragment, container, false);

            tvTitle = view.FindViewById<TextView>(Resource.Id.tvTitle);
            tvGenre = view.FindViewById<TextView>(Resource.Id.tvGenre);
            tvReleaseDate = view.FindViewById<TextView>(Resource.Id.tvReleaseDate);
            tvOverview = view.FindViewById<TextView>(Resource.Id.tvOverview);
            ivMovieDetail = view.FindViewById<ImageView>(Resource.Id.ivMovieDetail);

            
            if (movieDetails != null)
            {
                tvTitle.Text = movieDetails.Title;
                tvGenre.Text = FormatingGenres(movieDetails.Genres);
                tvReleaseDate.Text = FormatingStringDate(movieDetails.ReleaseDate);
                tvOverview.Text = movieDetails.Overview;

                //Library to work with images
                try
                {
                    Picasso.With(Activity)
                                .Load(movieDetails.ImageUrl)
                                .Into(ivMovieDetail);
                }
                catch (System.Exception)
                {
                    Toast.MakeText(Activity, "Error loading image", ToastLength.Long).Show();                    
                }

                //Set Movie Title in toolbar title
                var toolbar = ((Android.Support.V7.App.AppCompatActivity)Activity).SupportActionBar;
                toolbar.Title = movieDetails.Title;

            }

            return view;
        }

        
        private string FormatingStringDate(string date)
        {
            DateTime oDate = Convert.ToDateTime(date);

            string result = oDate.ToString("MMMM", CultureInfo.InvariantCulture) + " " + oDate.Day.ToString() + ", " + oDate.Year.ToString();

            return result;
        }
        
        private string FormatingGenres(List<Genre> genres)
        {
            string result = string.Empty;

            foreach (var g in genres)
            {
                result += string.Format("{0}{1}", g.name, ", ");
            }

            //removing last ','
            if (result != string.Empty)
            {
                result = result.Remove(result.LastIndexOf(","));
            }

            return result;

        }


    }
}