using UnifyMe.Core.Models;
using UnifyMe.Core.UserPreferences;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using UnifyMe.Core.Events;
using System;
using UnifyMe.Core.Enums;
using System.Windows.Media;
using UnifyMe.Core.Classes;

namespace UnifyMe.UIElements.Windows
{
    public sealed partial class Settings : UserControl
    {
        private static Settings Instance;
        private SettingsModel Model;
        public Settings()
        {
            this.Model = PreferencesManager.GetModel<SettingsModel>(nameof(SettingsModel));
            if (this.Model == null)
                this.Model = new SettingsModel();

            this.DataContext = this.Model;
            this.InitializeComponent();
            this.Model.PropertyChanged += this.Model_PropertyChanged;
            Instance = this;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.Model.ApplicationBackground))
            {
                ColorInfo tempForeGround = this.GetContrastForeground(this.Model.ApplicationBackground.Color);
                this.Model.FontColor = tempForeGround;
            }
            PreferencesManager.SaveModel<SettingsModel>(nameof(SettingsModel), this.Model);
            EventsManager.Instance.Raise(EventsNameEnums.OnSettingsRefresh, EventArgs.Empty);
        }

        private ColorInfo GetContrastForeground(Color color)
        {
            int d = 0;

            // Counting the perceptive luminance - human eye favors green color... 
            double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            if (luminance > 0.5)
                d = 0; // bright colors - black font
            else
                d = 255; // dark colors - white font

            Color foregroundColor = Color.FromRgb(Convert.ToByte(d), Convert.ToByte(d), Convert.ToByte(d));
            return new ColorInfo(null, foregroundColor);
        }
        #region Run
        public static DependencyProperty RunProperty =
            DependencyProperty.Register("Run",
            typeof(bool),
            typeof(Settings),
            new PropertyMetadata(default(bool), OnRunChanged));

        private static void OnRunChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue;
            if (value)
                Instance.Start();
            else
                Instance.Stop();
        }

        private void Stop()
        {
            this.grdMain.Visibility = Visibility.Collapsed;
        }

        private void Start()
        {
            this.grdMain.Visibility = Visibility.Visible;
        }

        public bool Run
        {
            get { return (bool)GetValue(RunProperty); }
            set { SetValue(RunProperty, value); }
        }
        #endregion
    }
    
}
