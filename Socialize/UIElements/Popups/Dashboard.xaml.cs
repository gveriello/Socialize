using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UnifyMe.Core.Classes;
using UnifyMe.Core.Managers;
using UnifyMe.Core.Models;
using Windows.Devices.Geolocation;

namespace UnifyMe.UIElements.Popups
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        private static Dashboard Instance;
        DashboardModel Model;
        public Dashboard()
        {
            InitializeComponent();
            Instance = this;
            this.Model = new DashboardModel();
            this.DataContext = this.Model;
            InitializeModel();
            this.SizeChanged += this.Dashboard_SizeChanged;
        }

        private void Dashboard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //this.grdNews.Height = this.Height - 50;
        }

        private void InitializeModel()
        {
            this.Model.GeolocatorObject = new Geolocator();
        }

        #region Run
        public static DependencyProperty RunProperty =
            DependencyProperty.Register("Run",
            typeof(bool),
            typeof(Dashboard),
            new PropertyMetadata(default(bool), OnRunChanged));

        private static void OnRunChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue;
            if (value)
                Instance.StartService();
            else
                Instance.StopService();
        }

        public bool Run
        {
            get { return (bool)GetValue(RunProperty); }
            set { SetValue(RunProperty, value); }
        }

        private void StopService()
        {
            grdMain.Visibility = Visibility.Collapsed;
        }

        private async void StartService()
        {
            grdMain.Visibility = Visibility.Visible;

            if (this.Model.HasWeatherInfo.Value)
                return;

            await GetCurrentPosition();
            InitializeWeather();
            InitializeNews();
        }

        private async Task GetCurrentPosition()
        {
            this.Model.gp = await this.Model.GeolocatorObject.GetGeopositionAsync();
        }

        private void InitializeNews()
        {
            if (this.Model.CurrentWeather == null)
                return;

            pbLoadedWidget.Visibility = Visibility.Visible;

            this.Model.NewsSource = RequestManager.GetNews(this.Model.CityName);

            pbLoadedWidget.Visibility = Visibility.Collapsed;
        }

        private async void InitializeWeather()
        {
            if (Model.gp == null)
                return;

            pbLoadedWidget.Visibility = Visibility.Visible;
            this.Model.CurrentWeather = null;
            Model.CurrentWeather = RequestManager.GetWheather(Model.gp.Coordinate.Point.Position.Longitude, Model.gp.Coordinate.Point.Position.Latitude);
            pbLoadedWidget.Visibility = Visibility.Collapsed;
        }
        #endregion

        private void Expander_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Expander gridSender = (Expander)sender;
            Process.Start(new ProcessStartInfo(gridSender.Tag.ToString()));
        }
    }


}
