using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V4.View;
using Com.Ittianyu.Bottomnavigationviewex;
using System;
using WeatherApp.Adapter;
using WeatherApp.Fragments;
using Android.Graphics;
using Android;
using Android.Support.V4.App;

namespace WeatherApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        //Views
        ViewPager viewpager;
        BottomNavigationViewEx bnve;

        //Fragments
        HomeFragement homeFragment = new HomeFragement();
        //MapFragment ratingsFragment = new MapFragment();
        SearchFragment earningsFragment = new SearchFragment();
        HomeFragement accountFragment = new HomeFragement();

        //PermissionRequest
        const int RequestID = 0;
        readonly string[] permissionsGroup =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
        };


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            ConnectViews();
            CheckSpecialPermission();
        }

        void ConnectViews()
        {
            bnve = (BottomNavigationViewEx)FindViewById(Resource.Id.bnve);
            bnve.EnableItemShiftingMode(false);
            bnve.EnableShiftingMode(false);
            var img0 = bnve.GetIconAt(0);
            var txt0 = bnve.GetLargeLabelAt(0);
            img0.SetColorFilter(Color.Rgb(24, 191, 242));
            txt0.SetTextColor(Color.Rgb(24, 191, 242));

            bnve.NavigationItemSelected += Bnve_NavigationItemSelected;
            viewpager = (ViewPager)FindViewById(Resource.Id.viewpager);
            viewpager.OffscreenPageLimit = 3;
            viewpager.BeginFakeDrag();

            SetupViewPager();
        }

        private void Bnve_NavigationItemSelected(object sender, Android.Support.Design.Widget.BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            if (e.Item.ItemId == Resource.Id.action_earning)
            {
                viewpager.SetCurrentItem(1, true);
                BnveToAccentColor(1);
            }
            else if (e.Item.ItemId == Resource.Id.action_home)
            {
                viewpager.SetCurrentItem(0, true);
                BnveToAccentColor(0);
               
            }
            //else if (e.Item.ItemId == Resource.Id.action_rating)
            //{
            //    viewpager.SetCurrentItem(2, true);
            //    BnveToAccentColor(2);

            //}
            else if(e.Item.ItemId == Resource.Id.action_account)
            {
                viewpager.SetCurrentItem(3, true);
                BnveToAccentColor(3);
            }
          
        }

        void BnveToAccentColor(int index)
        {
            //Set all to white
            var img = bnve.GetIconAt(1);
            var txt = bnve.GetLargeLabelAt(1);
            img.SetColorFilter(Color.Rgb(255, 255, 255));
            txt.SetTextColor(Color.Rgb(255, 255, 255));

            var img0 = bnve.GetIconAt(0);
            var txt0 = bnve.GetLargeLabelAt(0);
            img0.SetColorFilter(Color.Rgb(255, 255, 255));
            txt0.SetTextColor(Color.Rgb(255, 255, 255));

            var img2 = bnve.GetIconAt(2);
            var txt2 = bnve.GetLargeLabelAt(2);
            img2.SetColorFilter(Color.Rgb(255, 255, 255));
            txt2.SetTextColor(Color.Rgb(255, 255, 255));

            var img3 = bnve.GetIconAt(3);
            var txt3 = bnve.GetLargeLabelAt(3);
            img2.SetColorFilter(Color.Rgb(255, 255, 255));
            txt2.SetTextColor(Color.Rgb(255, 255, 255));

            //Sets Accent Color
            var imgindex = bnve.GetIconAt(index);
            var textindex = bnve.GetLargeLabelAt(index);
            imgindex.SetColorFilter(Color.Rgb(24, 191, 242));
            textindex.SetTextColor(Color.Rgb(24, 191, 242));

        }

        private void SetupViewPager()
        {
            ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
              adapter.AddFragment(homeFragment, "Home");
            adapter.AddFragment(earningsFragment, "Earnings");
          //  adapter.AddFragment(ratingsFragment, "Rating");
            adapter.AddFragment(accountFragment, "Account");
            viewpager.Adapter = adapter;
        }

        bool CheckSpecialPermission()
        {
            bool permissionGranted = false;
            if(ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted && 
                ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Android.Content.PM.Permission.Granted)
            {
                RequestPermissions(permissionsGroup, RequestID);
            }
            else
            {
                permissionGranted = true;
            }

            return permissionGranted;
        }

    }
}