using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Net.Http;
using System;
using Newtonsoft.Json.Linq;
using System.Globalization;
using WeatherApp.Fragments;
using System.Net;
using System.IO;
using Android.Graphics;
using Plugin.Connectivity;
using Android.Views;
using Android.Content;


namespace WeatherApp.Fragments
{
    public class SearchFragment : Android.Support.V4.App.Fragment
    {

        Button getWeatherButton;
        TextView placeTextView;
        TextView temperatureTextView;
        TextView weatherDescriptionTextView, humidityTextView, temp_min, temp_max;
        EditText cityNameEditText;
        ImageView weatherImageView;
        RelativeLayout infoweather;

        ProgressDialogueFragment progressDialogue;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
          View view = inflater.Inflate(Resource.Layout.search_layout, container, false);


            infoweather = (RelativeLayout)view.FindViewById(Resource.Id.infoweather);
            infoweather.Visibility = Android.Views.ViewStates.Gone;


            cityNameEditText = (EditText)view.FindViewById(Resource.Id.cityNameText);
            placeTextView = (TextView)view.FindViewById(Resource.Id.placeText);
          //  humidityTextView = (TextView)view.FindViewById(Resource.Id.humidity);
            //temp_min = (TextView)view.FindViewById(Resource.Id.temp_min);
            //temp_max = (TextView)view.FindViewById(Resource.Id.temp_max);
            temperatureTextView = (TextView)view.FindViewById(Resource.Id.temperatureTextView);
            weatherDescriptionTextView = (TextView)view.FindViewById(Resource.Id.weatherDescriptionText);
            weatherImageView = (ImageView)view.FindViewById(Resource.Id.weatherImage);
            getWeatherButton = (Button)view.FindViewById(Resource.Id.getWeatherButton);

            getWeatherButton.Click += GetWeatherButton_Click;

            GetWeather("Hanoi");
            return view;


        }

        private void GetWeatherButton_Click(object sender, System.EventArgs e)
        {
            string place = cityNameEditText.Text;
            infoweather.Visibility = Android.Views.ViewStates.Visible;
            GetWeather(place); 
            cityNameEditText.Text = "";
        }

        async void GetWeather(string place)
        {
            string apiKey = "7171a9e68df919e7ad4119110c45d487";
            string apiBase = "https://api.openweathermap.org/data/2.5/weather?q=";
            string unit = "metric";
            // Context context;

            if (string.IsNullOrEmpty(place))
            {
                Toast.MakeText(Context, "please enter a valid city name", ToastLength.Short).Show();
                return;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                Toast.MakeText(Context, "No internet connection", ToastLength.Short).Show();
                return;
            }

            ShowProgressDialogue("Fetching weather...");

            // Asynchronous API call using HttpClient
            string url = apiBase + place + "&appid=" + apiKey + "&units=" + unit;
            var handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string result = await client.GetStringAsync(url);

            Console.WriteLine(result);

            var resultObject = JObject.Parse(result);
            string weatherDescription = resultObject["weather"][0]["description"].ToString();
            string icon = resultObject["weather"][0]["icon"].ToString();
            string temperature = resultObject["main"]["temp"].ToString();
            //string tempmind = resultObject["main"]["temp_min"].ToString();
            //string tempmax = resultObject["main"]["temp_max"].ToString();
           // string humidity = resultObject["main"]["humidity"].ToString();
            string placename = resultObject["name"].ToString();
            string country = resultObject["sys"]["country"].ToString();
            weatherDescription = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(weatherDescription);

            weatherDescriptionTextView.Text = weatherDescription;
            placeTextView.Text = placename + ", " + country;
            temperatureTextView.Text = temperature + "°C";
            //temp_min.Text = "Temp_min: " + tempmind + "°C";
            //temp_max.Text = "Temp_max: " + tempmax + "°C";
          //  humidityTextView.Text = humidity;


            // Download Image using WebRequest
            string ImageUrl = "http://openweathermap.org/img/w/" + icon + ".png";
            System.Net.WebRequest request = default(System.Net.WebRequest);
            request = WebRequest.Create(ImageUrl);
            request.Timeout = int.MaxValue;
            request.Method = "GET";

            WebResponse response = default(WebResponse);
            response = await request.GetResponseAsync();
            MemoryStream ms = new MemoryStream();
            response.GetResponseStream().CopyTo(ms);
            byte[] imageData = ms.ToArray();

            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            weatherImageView.SetImageBitmap(bitmap);


            ClossProgressDialogue();
        }


        void ShowProgressDialogue(string status)
        {
            progressDialogue = new ProgressDialogueFragment(status);
            var trans = FragmentManager.BeginTransaction();
            progressDialogue.Cancelable = false;
            progressDialogue.Show(trans, "progress");
        }



        void ClossProgressDialogue()
        {
            if (progressDialogue != null)
            {
                progressDialogue.Dismiss();
                progressDialogue = null;
            }
        }
    }
}