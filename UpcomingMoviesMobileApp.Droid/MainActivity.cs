using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using UpcomingMoviesMobileApp.Droid.Fragments;
using UpcomingMoviesMobileApp.Shared.Models;

namespace UpcomingMoviesMobileApp.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "UPCM APP";
            SetSupportActionBar(toolbar);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            //if first time loading system go to popular fragment
            if (savedInstanceState == null)
                DisplaySelectedScreen(Resource.Id.nav_popular);
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }


        public bool OnNavigationItemSelected(IMenuItem item)
        {
            //calling the method to display selected screen
            DisplaySelectedScreen(item.ItemId);
            return true;
        }

        private void DisplaySelectedScreen(int itemId)
        {
            //creating fragment object
            Android.Support.V4.App.Fragment fragment = null;

            //initializing the fragment object which is selected 

            switch (itemId)
            {
                case Resource.Id.nav_popular:
                    fragment = MoviesFragment.NewInstance(MovieCategory.Popular);
                    break;

                case Resource.Id.nav_toprated:
                    fragment = MoviesFragment.NewInstance(MovieCategory.TopRated);
                    break;

                case Resource.Id.nav_upcoming:
                    fragment = MoviesFragment.NewInstance(MovieCategory.Upcoming);
                    break;

                case Resource.Id.nav_now_playing:
                    fragment = MoviesFragment.NewInstance(MovieCategory.NowPlaying);
                    break;
            }

            //replacing the fragment
            if (fragment != null)
            {
                Android.Support.V4.App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.content_frame, fragment);
                ft.AddToBackStack(null);
                ft.Commit();
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);

        }

       


    }
}

