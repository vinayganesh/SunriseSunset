using Caliburn.Micro;
using SunriseSunsetUWP.IServices;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using Windows.Devices.Geolocation;
using SunriseSunsetUWP.DataContract;
using Newtonsoft.Json;
using SunriseSunsetUWP.Helpers;

namespace SunriseSunsetUWP.ViewModels
{
    public class LandingPageViewModel : Screen
    {
        #region Properties

        string _sunrise;
        public string Sunrise
        {
            get { return _sunrise; }
            set
            {
                if (!value.Equals(_sunrise))
                {
                    _sunrise = value;
                    NotifyOfPropertyChange(()=>Sunrise);
                }
            }
        }

        string _sunSet;
        public string Sunset
        {
            get { return _sunSet; }
            set
            {
                if (!value.Equals(_sunSet))
                {
                    _sunSet = value;
                    NotifyOfPropertyChange(()=>Sunset);
                }
            }
        }

        string _dayLength;
        public string DayLength
        {
            get { return _dayLength; }
            set
            {
                if (!value.Equals(_dayLength))
                {
                    _dayLength = value;
                    NotifyOfPropertyChange(() => DayLength);
                }
            }
        }

        string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                if (!value.Equals(_location))
                {
                    _location = value;
                    NotifyOfPropertyChange(() => Location);
                }
            }
        }

        string _solarNoon;
        public string SolarNoon
        {
            get { return _solarNoon; }
            set
            {
                if (!value.Equals(_solarNoon))
                {
                    _solarNoon = value;
                    NotifyOfPropertyChange(() => SolarNoon);
                }
            }
        }

        string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (!value.Equals(_currentTime))
                {
                    _currentTime = value;
                    NotifyOfPropertyChange(() => CurrentTime);
                }
            }
        }

        bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (!value.Equals(_isActive))
                {
                    _isActive = value;
                    NotifyOfPropertyChange(() => IsActive);
                }
            }
        }

        public IGeoCoderService _geoCoderService;

        static string BaseURL = "http://api.sunrise-sunset.org/json";

        #endregion

        public LandingPageViewModel(IGeoCoderService geoCoderService)
        {
            _geoCoderService = geoCoderService;
        }

        protected override void OnInitialize()
        {
            Display();
        }

        /// <summary>
        /// Build URL string using Latitude and Longitude
        /// </summary>
        /// <param name="latlng"></param>
        /// <returns></returns>
        public string BuildUrl(Tuple<double, double> latlng)
        {
            return BaseURL + "?lat=" + latlng.Item1.ToString() + "&lng=" + latlng.Item2.ToString()+ "&date=today";
        }

        /// <summary>
        /// Deserialize the json data received from the server
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<LatLngContract.RootObject> GetSunData(string url)
        {
            try
            {
                var jsonString = await HttpClientHelper.SendHttpRequest(url);
                var data = JsonConvert.DeserializeObject<LatLngContract.RootObject>(jsonString);
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// The entry point into the application
        /// </summary>
        public async void Display()
        {
            try
            {
                IsActive = true;

                //get current location
                Geoposition position = await _geoCoderService.GetCurrentGeoPosition();
                //get lat long from current location
                var latlng = _geoCoderService.GetLatLongFromLocation(position);
                //build url
                string url = BuildUrl(latlng);
                //send http request and deserialize the data
                var data = await GetSunData(url);

                if (data != null)
                {
                    //assign it to the UI
                    Location = "Current Location is " + await _geoCoderService.GetLocationFromLatLong();
                    CurrentTime = "Current Time is " + DateTime.Now.ToString("hh:mm tt");
                    Sunrise = "Sunrise at " + _geoCoderService.TimeZoneConverter(data.results.sunrise);
                    Sunset = "Sunset at " + _geoCoderService.TimeZoneConverter(data.results.sunset);
                    DayLength = "Today's Length " + (data.results.day_length) +" hours";
                    SolarNoon = "The Solar Noon time today " + _geoCoderService.TimeZoneConverter(data.results.solar_noon);
                   
                }
                IsActive = false;
            }
            catch (Exception ex)
            {
                IsActive = false;
                Debug.WriteLine(ex.Message);
            }
        }

       
    }
}
