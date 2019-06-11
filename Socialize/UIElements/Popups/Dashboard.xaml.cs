using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UnifyMe.Core.Classes;
using UnifyMe.Core.Managers;
using Windows.Devices.Geolocation;

namespace UnifyMe.UIElements.Popups
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        private static Dashboard Instance;
        private static bool alrealyDownloaded;
        Geolocator gl;
        public Dashboard()
        {
            InitializeComponent();
            Instance = this;
            gl = new Geolocator();
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

            if (alrealyDownloaded)
                return;

            Geoposition gp = await this.GetCurrentPosition();

            if (gp != null)
            {
                ResponseWeather currentWheater = RequestManager.GetWheatherAsync(gp.Coordinate.Point.Position.Longitude, gp.Coordinate.Point.Position.Latitude);
                tbMeteoAttuale.Visibility = Visibility.Collapsed;
                if (currentWheater != null)
                {
                    alrealyDownloaded = true;
                    this.DataContext = currentWheater;
                    string description = currentWheater?.weather.Select(i => i.main).First();
                    tbDescription.Text = description;
                    tbMeteoAttuale.Visibility = Visibility.Visible;
                    tbActualTemp.Text = $"{((int)(currentWheater.main.temp / 10))} °";
                    //string country = currentWheater?.name;
                }
            }

            tbMessage.Text = this.GetRandomWelcomeText();
            tbMessage.Visibility = (gp == null) ? Visibility.Visible : Visibility.Collapsed;
            tbMigliorEsperienzaUtente.Visibility = (gp == null) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task<Geoposition> GetCurrentPosition()
        {
            try
            {
                Geoposition gp = await gl.GetGeopositionAsync();
                return gp;
            }
            catch
            {
                return null;
            }
        }

        private string GetRandomWelcomeText()
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
        #endregion
    }
}
