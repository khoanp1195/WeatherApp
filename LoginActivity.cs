using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeatherApp.EventListener;
using Xamarin.Essentials;

namespace WeatherApp
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        TextView registerTxt;
        EditText edtEmail, edtPassword;
        Button btnLogin;
        ConstraintLayout constraintLayout;
        TaskCompletionListener taskCompletionListener = new TaskCompletionListener();
        FirebaseAuth mAuth;
        string email, password;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            // Create your application here
            Control();
             IntializeDatabase();

        }

        void Control()
        {
            edtEmail = FindViewById<EditText>(Resource.Id.editEmail);
            edtPassword = FindViewById<EditText>(Resource.Id.editPass);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += BtnLogin_Click;


            registerTxt = FindViewById<TextView>(Resource.Id.createText);
            registerTxt.Click += RegisterTxt_Click;
        }

        void IntializeDatabase()
        {
            var app = FirebaseApp.InitializeApp(this);

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                .SetApplicationId("weatherapp-aa6af")
                .SetApiKey("AIzaSyBucpwSsX2A5hpK7LR_UCRZ87H-9wMm7dY")
                .SetDatabaseUrl("https://weatherapp-aa6af-default-rtdb.asia-southeast1.firebasedatabase.app/")
                .SetStorageBucket("weatherapp-aa6af.appspot.com")
                .Build();

                app = FirebaseApp.InitializeApp(this, options);
                mAuth = FirebaseAuth.Instance;

            }
            else
            {

                mAuth = FirebaseAuth.Instance;
            }

        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {

            email = edtEmail.Text;
            password = edtPassword.Text;

            if (!email.Contains('@'))
            {
                Snackbar.Make(constraintLayout, "Please Enter Email Have @", Snackbar.LengthShort).Show();
                return;
            }
            else if (password.Length < 6)
            {
                Snackbar.Make(constraintLayout, "Please Enter Password > 6", Snackbar.LengthShort).Show();
                return;
            }

            loginUser(email, password);


        }


        void loginUser(string email, string password)
        {
            taskCompletionListener.Success += taskCompletionListener_Success;
            taskCompletionListener.Failure += taskCompletionListener_Failure;
            mAuth.SignInWithEmailAndPassword(email, password)
                    .AddOnSuccessListener(taskCompletionListener)
                    .AddOnFailureListener(taskCompletionListener);
        }

        private void taskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(constraintLayout, "SignIn Failed !! Please Again", Snackbar.LengthShort).Show();
        }

        private void taskCompletionListener_Success(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);


        }

        private void RegisterTxt_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterActivity));
            StartActivity(intent);
        }
    }
}