using Android.OS;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpcomingMoviesMobileApp.Shared.Models;
using UpcomingMoviesMobileApp.Shared.Services;

namespace UpcomingMoviesMobileApp.Droid.Fragments
{
    public class MoviesFragment : Fragment
    {
        MoviesService service = new MoviesService();


        MovieConfig moviesConfig;
        List<Movie> movies;
        int pageNumber = 1;
        int category = 1;

        //widgets
        ProgressBar prgMovies;
        EditText searchMovie;
        ListView lvMovies;
        Button btnPrevious, btnNext;
        TextView tvPage;

        //Adapter
        MoviesAdapter Adapter;

        public static MoviesFragment NewInstance(MovieCategory movieCategory)
        {
            Bundle args = new Bundle();
            args.PutInt("movieCategory", (int)movieCategory);

            return new MoviesFragment { Arguments = args };
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.movies_fragment, container, false);

            prgMovies = view.FindViewById<ProgressBar>(Resource.Id.prgMovies);
            searchMovie = view.FindViewById<EditText>(Resource.Id.searchMovie);
            lvMovies = view.FindViewById<ListView>(Resource.Id.lvMovies);
            btnPrevious = view.FindViewById<Button>(Resource.Id.btnPrevious);
            btnNext = view.FindViewById<Button>(Resource.Id.btnNext);
            tvPage = view.FindViewById<TextView>(Resource.Id.tvPage);

            //Hide 
            searchMovie.Visibility = ViewStates.Gone;
            btnPrevious.Visibility = ViewStates.Gone;
            btnNext.Visibility = ViewStates.Gone;
            tvPage.Visibility = ViewStates.Gone;

            //Events
            lvMovies.ItemClick += LvMovies_ItemClick;
            btnPrevious.Click += (s, e) =>
            {
                if (pageNumber > 1)
                {
                    pageNumber -= 1;
                    Activity.RunOnUiThread(async () =>
                    {
                        await GetMovies(pageNumber);
                    });
                }
            };

            btnNext.Click += (s, e) =>
            {
                pageNumber += 1;
                Activity.RunOnUiThread(async () =>
                {
                    await GetMovies(pageNumber);
                });
            };
                        
            searchMovie.TextChanged += SearchMovie_TextChanged;
            searchMovie.KeyPress += SearchMovie_KeyPress;
                        
            
            return view;
        }

        private void SearchMovie_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Enter)
            {
                //Close keyboard
                InputMethodManager imm = (InputMethodManager)Context.GetSystemService(Android.Content.Context.InputMethodService);
                imm.HideSoftInputFromWindow(searchMovie.WindowToken, 0);
            }
            else
            {
                e.Handled = false;
            }
        }

        private void SearchMovie_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = searchMovie.Text;

            //Filtering data 
            if (movies != null)
            {
                List<Movie> moviesFiltered = movies.Where(m => m.title.ToLower().Contains(searchText.ToLower())).ToList();
                SetupMoviesAdapter(moviesFiltered);
            }
        }

        public async override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            if (Arguments != null)
            {
                if (((Android.Support.V7.App.AppCompatActivity)Activity).SupportActionBar != null)
                {
                    category = Arguments.GetInt("movieCategory");
                    var toolbar = ((Android.Support.V7.App.AppCompatActivity)Activity).SupportActionBar;

                    if (category == (int)MovieCategory.Popular)
                        toolbar.Title = Context.GetString(Resource.String.popular_movies);

                    else if (category == (int)MovieCategory.TopRated)
                        toolbar.Title = Context.GetString(Resource.String.top_rated);

                    else if (category == (int)MovieCategory.Upcoming)
                        toolbar.Title = Context.GetString(Resource.String.upcoming);

                    else if (category == (int)MovieCategory.NowPlaying)
                        toolbar.Title = Context.GetString(Resource.String.now_playing);
                }
            }

            await GetMovies(pageNumber);
        }

        private async Task GetMovies(int pageNumber)
        {
            moviesConfig = await service.GetMovies(category, pageNumber);

            if (moviesConfig != null && moviesConfig.results.Count > 0)
            {
                movies = moviesConfig.results;
                SetupMoviesAdapter(movies);
                ShowControls();
                SetPageNumber(pageNumber);
            }
        }

        private void SetPageNumber(int pageNumber)
        {
            tvPage.Text = Context.GetString(Resource.String.page) + " " + pageNumber;
        }

        private void SetupMoviesAdapter(List<Movie> movies)
        {
            Adapter = new MoviesAdapter(Activity, movies);
            lvMovies.Adapter = Adapter;
        }

        private void ShowControls()
        {
            if (pageNumber > 1)
                btnPrevious.Visibility = ViewStates.Visible;
            else
                btnPrevious.Visibility = ViewStates.Gone;

            if (pageNumber >= moviesConfig.total_pages)
                btnNext.Visibility = ViewStates.Gone;
            else
                btnNext.Visibility = ViewStates.Visible;

            searchMovie.Visibility = ViewStates.Visible;
            prgMovies.Visibility = ViewStates.Gone;
            tvPage.Visibility = ViewStates.Visible;

        }

        private void LvMovies_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Movie movie = movies[e.Position];


            MovieDetails movieDetails = new MovieDetails
            {
                Title = movie.title,
                Genres = movie.genres,
                ReleaseDate = movie.release_date,
                Overview = movie.overview,
                ImageUrl = "https://image.tmdb.org/t/p/w500" + movie.poster_path

            };


            //Pass data to CheckOutFormFragment
            Bundle args = new Bundle();
            args.PutString("MovieDetail", JsonConvert.SerializeObject(movieDetails));

            Fragment fragment = new MovieDetailsFragment();

            //Add arguments to fragment
            fragment.Arguments = args;

            //Go to Fragment
            if (fragment != null)
            {
                FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.content_frame, fragment);
                ft.AddToBackStack(null);
                ft.Commit();
            }

        }
               
    }
}