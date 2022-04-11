using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherApp
{
    [Activity(Label = "WelcomeActivity",NoHistory =true, MainLauncher = false) ]
    public class WelcomeActivity : Activity
    {
        TextView txtSigin;
        Button btnRegister;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.wait);

            txtSigin = FindViewById<TextView>(Resource.Id.txtSigin);
            btnRegister = FindViewById<Button>(Resource.Id.btnRegister);


            txtSigin.Click += TxtSigin_Click;
            btnRegister.Click += BtnRegister_Click;
            // Create your application here
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterActivity));
            StartActivity(intent);
        }

        private void TxtSigin_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }
    }
}