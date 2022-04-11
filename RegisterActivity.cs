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

using System.Text.RegularExpressions;
using Android.Util;

using Java.Util;
using Firebase.Auth;
using Firebase.Database;
using Android.Support.Constraints;
using WeatherApp.EventListener;
using Firebase;
using Android.Support.Design.Widget;

namespace WeatherApp
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        EditText fullname, email, password, phone;
        Button btnRegister;
        TextView signin;
        FirebaseAuth mAuth;
        FirebaseDatabase mDatabase;
        ConstraintLayout registerLayout;
        TaskCompletionListener taskCompletionListener = new TaskCompletionListener();
        string fullName, Email, Password, Phone;

        ISharedPreferences preferences = Application.Context.GetSharedPreferences("userInfo", FileCreationMode.Private);
        ISharedPreferencesEditor editor;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);
        ConnectControl();

            IntializeDatabase();
            SaveToShareReference();
            mAuth = FirebaseAuth.Instance;

            // Create your application here
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
                mDatabase = FirebaseDatabase.GetInstance(app);
            }
            else
            {
                mDatabase = FirebaseDatabase.GetInstance(app);
            }
            DatabaseReference dbrf = mDatabase.GetReference("UserSupport");
            dbrf.SetValue("Ticket");

            Toast.MakeText(this, "Make Test", ToastLength.Short).Show();
        }


        void ConnectControl()
        {
            fullname = (EditText)FindViewById(Resource.Id.editName);
            email = (EditText)FindViewById(Resource.Id.edt_email);
            signin = (TextView)FindViewById(Resource.Id.signin);
            signin.Click += Signin_Click;
            password = (EditText)FindViewById(Resource.Id.edt_password);
            phone = (EditText)FindViewById(Resource.Id.edt_phone);
            registerLayout = (ConstraintLayout)FindViewById(Resource.Id.register_layout);
            btnRegister = (Button)FindViewById(Resource.Id.button);
            btnRegister.Click += btnRegisterClick;
        }

        private void Signin_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }

        private void btnRegisterClick(object sender, EventArgs e)
        {

            fullName = fullname.Text;
            Email = email.Text;
            Password = password.Text;
            Phone = phone.Text;

            if (fullName.Length < 6)
            {
                Snackbar.Make(registerLayout, "Please Enter FullName >6 ", Snackbar.LengthShort).Show();
                return;
            }

            else if (Phone.Length < 6)
            {
                Snackbar.Make(registerLayout, "Sorry Please Again ", Snackbar.LengthShort).Show();
                return;
            }
            else if (!Email.Contains('@'))
            {
                Snackbar.Make(registerLayout, "Sorry Please Enter Email Have @ ", Snackbar.LengthShort).Show();
                return;
            }
            else if (Password.Length < 6)
            {
                Snackbar.Make(registerLayout, "Sorry Please Again ", Snackbar.LengthShort).Show();
                return;
            }

            RegisterUser(fullName, Phone, Email, Password);
        }

        void RegisterUser(string fullname, string phone, string email, string password)
        {
            taskCompletionListener.Success += taskCompletionListener_Success;
            taskCompletionListener.Failure += taskCompletionListener_Failure;

            mAuth.CreateUserWithEmailAndPassword(email, password)
                .AddOnSuccessListener(this, taskCompletionListener)
                .AddOnFailureListener(this, taskCompletionListener);
        }

        private void taskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(registerLayout, "Register Fail", Snackbar.LengthShort).Show();
        }

        private void taskCompletionListener_Success(object sender, EventArgs e)
        {
            Snackbar.Make(registerLayout, "Register Success", Snackbar.LengthShort).Show();
            HashMap userMap = new HashMap();

            userMap.Put("email", Email);
            userMap.Put("fullname", fullName);
            userMap.Put("phone", Phone);

            DatabaseReference userReference = mDatabase.GetReference("users/" + Phone);
            userReference.SetValue(userMap);
        }

        void SaveToShareReference()
        {
            ISharedPreferences preferences = Application.Context.GetSharedPreferences("userInfo", FileCreationMode.Private);//FileCreationMode tệp thông tin người dùng chế độ private
            ISharedPreferencesEditor editor;
            editor = preferences.Edit();
            editor.PutString("email", (string)email);
            editor.PutString("fulname", (string)fullname);
            editor.PutString("phone", (string)phone);

            editor.Apply();


        }

        void RetriveData()
        {
            string email = preferences.GetString("email", "");
        }

    }
    }
