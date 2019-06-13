using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UnifyMe.Core.Classes;
using Windows.Devices.Geolocation;

namespace UnifyMe.Core.Models
{
    public class DashboardModel: Model
    {
        private BitmapImage _iconWeather;
        private string _cityName;
        private Geoposition _gp;
        private ResponseWeather _currentWeather;
        private Geolocator _geolocatorObject;
        private string _welcomeMessage;
        private bool _hasWeatherInfo;
        private bool _notHasWheaterInfo;
        private bool? _notHasNews;
        private IList<ItemNews> _newsSource;
        private string _description;
        private string _actualTemp;

        public BitmapImage IconWeather
        {
            get { return _iconWeather; }
            set
            {
                _iconWeather = value;
                OnPropertyChanged(nameof(IconWeather));
            }
        }

        public string CityName
        {
            get { return _cityName; }
            set
            {
                if (value == _cityName)
                    return;

                _cityName = value;
                OnPropertyChanged(nameof(CityName));
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description)
                    return;

                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string ActualTemp
        {
            get { return _actualTemp; }
            set
            {
                if (value == _actualTemp)
                    return;

                _actualTemp = value;
                OnPropertyChanged(nameof(ActualTemp));
            }
        }

        public bool? HasWeatherInfo
        {
            get { return _hasWeatherInfo; }
            set {
                _hasWeatherInfo = this.gp != null && this.CurrentWeather != null;
                this.NotHasWeatherInfo = !_hasWeatherInfo;
                OnPropertyChanged(nameof(HasWeatherInfo));
            }
        }

        public bool NotHasWeatherInfo
        {
            get { return !this.HasWeatherInfo.Value; }
            set { OnPropertyChanged(nameof(NotHasWeatherInfo)); }
        }


        public bool? HasNews
        {
            get { return _notHasNews; }
            set
            {
                _notHasNews = value;
                OnPropertyChanged(nameof(HasNews));
            }
        }

        public string WelcomeMessage
        {
            get
            {
                IList<string> messages = new List<string>
                {
                    "Ciao, cosa posso fare per te? 😋",
                    "Bella giornata oggi, vero? 🌞",
                    "Hey! I tuoi amici ti stanno aspettando 🍻"
                };
                Random random = new Random();
                return messages.ElementAt(random.Next(0, messages.Count));
            }
        }

        public IList<ItemNews> NewsSource
        {
            get { return _newsSource; }
            set
            {
                _newsSource = value;
                if (_newsSource != null)
                    this.HasNews = _newsSource.Count > 0;

                OnPropertyChanged(nameof(NewsSource));
            }
        }

        public Geoposition gp
        {
            get { return this._gp; }
            set
            {
                this._gp = value;
                this.HasWeatherInfo = null;
                OnPropertyChanged(nameof(gp));
            }
        }

        public ResponseWeather CurrentWeather
        {
            get { return this._currentWeather; }
            set
            {
                this._currentWeather = value;

                this.HasWeatherInfo = null;

                if (_currentWeather != null)
                {
                    this.IconWeather = _currentWeather.logo;
                    this.CityName = _currentWeather.name;
                    this.Description = _currentWeather.weather.Select(i => i.main).First();
                    this.ActualTemp = $"{((int)_currentWeather.main?.temp) / 10} °";
                }

                OnPropertyChanged(nameof(CurrentWeather));
            }
        }

        public Geolocator GeolocatorObject
        {
            get { return this._geolocatorObject; }
            set
            {
                this._geolocatorObject = value;
                OnPropertyChanged(nameof(GeolocatorObject));
            }
        }
    }
}

