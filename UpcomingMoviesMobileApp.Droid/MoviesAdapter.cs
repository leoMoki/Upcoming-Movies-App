using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Squareup.Picasso;
using System;
using System.Collections.Generic;
using System.Globalization;
using UpcomingMoviesMobileApp.Shared.Models;

namespace UpcomingMoviesMobileApp.Droid
{
    public class MoviesAdapter : BaseAdapter
    {
                
        Context context;
        List<Movie> movies;
        List<Movie> moviesFilter;


        public MoviesAdapter(Context context, List<Movie> movies)
        {
            this.movies = movies;
            this.moviesFilter = movies;
            this.context = context;
        }

        public override int Count
        {
            get { return movies.Count; }
        }
                
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            MoviesAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as MoviesAdapterViewHolder;

            if (holder == null)
            {
                holder = new MoviesAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();

                //View
                view = inflater.Inflate(Resource.Layout.lv_item_moveis, parent, false);


                //holder
                holder.Title = view.FindViewById<TextView>(Resource.Id.tvTitle);
                holder.Genre = view.FindViewById<TextView>(Resource.Id.tvGenre);
                holder.ReleaseDate = view.FindViewById<TextView>(Resource.Id.tvReleaseDate);
                holder.ivMovie = view.FindViewById<ImageView>(Resource.Id.ivMovie);

                view.Tag = holder;
            }

            


            //fill items
            holder.Title.Text = movies[position].title;
            //formating genres
            holder.Genre.Text = FormatingGenres(movies[position].genres);
            holder.ReleaseDate.Text = FormatingStringDate(movies[position].release_date);

            //Library to work with images
            Picasso.With(context)
                .Load("https://image.tmdb.org/t/p/w500" + movies[position].poster_path)
                .Into(holder.ivMovie);

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

    class MoviesAdapterViewHolder : Java.Lang.Object
    {        
        public TextView Title { get; set; }
        public TextView Genre { get; set; }
        public TextView ReleaseDate { get; set; }
        public ImageView ivMovie { get; set; }
    }
}