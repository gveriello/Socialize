using UnifyMe.Core.Classes;
using UnifyMe.Core.Enums;
using UnifyMe.Core.Events;
using UnifyMe.Core.Models;
using UnifyMe.Core.UserPreferences;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UnifyMe
{
    public partial class UserHome : UserControl
    {
        private ServicesEnums lastServiceUsed;
        private ServicesContext Model;
        public UserHome()
        {
            this.Model = new ServicesContext();
            this.DataContext = this.Model;
            this.InitializeComponent();
            this.Loaded += this.UserHome_Loaded;
            //tbMessage.Text = this.GetRandomWelcomeText();
            //tbMessage.Visibility = Visibility.Visible;
            this.ppDashboard.Run = true;
            this.Model.PropertyChanged += this.Model_PropertyChanged;
        }

        private void UserHome_Loaded(object sender, RoutedEventArgs e)
        {
            this.btnWhatsapp.Click += this.BtnWhatsapp_Click;
            this.tbStopWhatsapp.PreviewMouseDown += this.TbStopWhatsapp_PreviewMouseDown;
            this.btnTelegram.Click += this.BtnTelegram_Click;
            this.tbStopTelegram.PreviewMouseDown += this.TbStopTelegram_PreviewMouseDown;
            this.btnSkype.Click += this.BtnSkype_Click;
            this.tbStopSkype.PreviewMouseDown += this.TbStopSkype_PreviewMouseDown;
            this.btnSlack.Click += this.BtnSlack_Click;
            this.tbStopSlack.PreviewMouseDown += this.TbStopSlack_PreviewMouseDown;
            this.btnSettings.Click += this.BtnSettings_Click;
            this.tbCloseSettings.PreviewMouseDown += this.TbCloseSettings_PreviewMouseDown;
            this.CheckUserPreferences();
            EventsManager.Instance.SubScribe(EventsNameEnums.OnSettingsRefresh, CheckUserPreferences);
        }
        private void CheckUserPreferences()
        {
            ColorInfo ApplicationBackground = ManagePreferences.GetPropertyFromModel<SettingsModel, ColorInfo>(nameof(SettingsModel), "ApplicationBackground", new ColorInfo(color_name: null, color: Colors.GreenYellow));
            ColorInfo ApplicationForeGround = ManagePreferences.GetPropertyFromModel<SettingsModel, ColorInfo>(nameof(SettingsModel), "FontColor", new ColorInfo(color_name: null, color: Colors.Black));
            ColorInfo ServicesBackground = ManagePreferences.GetPropertyFromModel<SettingsModel, ColorInfo>(nameof(SettingsModel), "ServiceBackgroundActive", new ColorInfo(color_name: null, color: Colors.White));

            bool WhatsappActive = ManagePreferences.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "WhatsappActive");
            bool TelegramActive = ManagePreferences.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "TelegramActive");
            bool SkypeActive = ManagePreferences.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "SkypeActive");
            bool SlackActive = ManagePreferences.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "SlackActive");

            this.Background = new SolidColorBrush(ApplicationBackground.Color);
            this.Foreground = new SolidColorBrush(ApplicationForeGround.Color);

            this.btnWhatsapp.Visibility = WhatsappActive ? Visibility.Visible : Visibility.Collapsed;
            this.btnTelegram.Visibility = TelegramActive ? Visibility.Visible : Visibility.Collapsed;
            this.btnSlack.Visibility = SlackActive ? Visibility.Visible : Visibility.Collapsed;
            this.btnSkype.Visibility = SkypeActive ? Visibility.Visible : Visibility.Collapsed;
        }


        #region Whatsapp
        private void BtnWhatsapp_Click(object sender, RoutedEventArgs e)
        {
            this.Model.WhatsappRun = true;
        }
        private void TbStopWhatsapp_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.WhatsappRun = false;
        }
        #endregion

        #region Telegram
        private void BtnTelegram_Click(object sender, RoutedEventArgs e)
        {
            this.Model.TelegramRun = true;
        }
        private void TbStopTelegram_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.TelegramRun = false;
        }
        #endregion

        #region Skype
        private void BtnSkype_Click(object sender, RoutedEventArgs e)
        {
            this.Model.SkypeRun = true;
        }
        private void TbStopSkype_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.SkypeRun = false;
        }
        #endregion

        #region Slack
        private void BtnSlack_Click(object sender, RoutedEventArgs e)
        {
            this.Model.SlackRun = true;
        }
        private void TbStopSlack_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.SlackRun = false;
        }
        #endregion

        #region Settings
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Model.SettingsRun = true;
        }
        private void TbCloseSettings_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Model.SettingsRun = false;
        }
        #endregion


        private void StopAllServices()
        {
            this.Model.WhatsappRun = false;
            this.Model.TelegramRun = false;
            this.Model.SkypeRun = false;
            this.Model.SlackRun = false;
            this.Model.SettingsRun = false;
        }

        private void CollapseAllServices()
        {
            this.ucWhatsapp.Visibility = Visibility.Collapsed;
            this.ucTelegram.Visibility = Visibility.Collapsed;
            this.ucSkype.Visibility = Visibility.Collapsed;
            this.ucSlack.Visibility = Visibility.Collapsed;
            this.ucSettings.Visibility = Visibility.Collapsed;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ColorInfo ApplicationBackground = ManagePreferences.GetPropertyFromModel<SettingsModel, ColorInfo>(nameof(SettingsModel), "ApplicationBackground", new ColorInfo(color_name: null, color: Colors.White));
            ColorInfo ServiceBackground = ManagePreferences.GetPropertyFromModel<SettingsModel, ColorInfo>(nameof(SettingsModel), "ServiceBackgroundActive", new ColorInfo(color_name: null, color: Colors.GreenYellow));
            bool newValue = false;

            if (e.PropertyName == nameof(this.Model.SkypeRun))
            {
                newValue = this.Model.SkypeRun;
                this.CollapseAllServices();
                this.lastServiceUsed = ServicesEnums.Skype;
                this.tbStopSkype.Visibility = (this.Model.SkypeRun) ? Visibility.Visible : Visibility.Collapsed;
                this.ucSkype.Visibility = (this.Model.SkypeRun) ? Visibility.Visible : Visibility.Collapsed;
                this.btnSkype.Background = new SolidColorBrush((this.Model.SkypeRun) ? ServiceBackground.Color : ApplicationBackground.Color);
            }

            if (e.PropertyName == nameof(this.Model.TelegramRun))
            {
                newValue = this.Model.TelegramRun;
                this.CollapseAllServices();
                this.lastServiceUsed = ServicesEnums.Telegram;
                this.tbStopTelegram.Visibility = (this.Model.TelegramRun) ? Visibility.Visible : Visibility.Collapsed;
                this.ucTelegram.Visibility = (this.Model.TelegramRun) ? Visibility.Visible : Visibility.Collapsed;
                this.btnTelegram.Background = new SolidColorBrush((this.Model.TelegramRun) ? ServiceBackground.Color : ApplicationBackground.Color);
            }

            if (e.PropertyName == nameof(this.Model.WhatsappRun))
            {
                newValue = this.Model.WhatsappRun;
                this.CollapseAllServices();
                this.lastServiceUsed = ServicesEnums.Whatsapp;
                this.tbStopWhatsapp.Visibility = (this.Model.WhatsappRun) ? Visibility.Visible : Visibility.Collapsed;
                this.ucWhatsapp.Visibility = (this.Model.WhatsappRun) ? Visibility.Visible : Visibility.Collapsed;
                this.btnWhatsapp.Background = new SolidColorBrush((this.Model.WhatsappRun) ? ServiceBackground.Color : ApplicationBackground.Color);
            }

            if (e.PropertyName == nameof(this.Model.SlackRun))
            {
                newValue = this.Model.SlackRun;
                this.CollapseAllServices();
                this.lastServiceUsed = ServicesEnums.Slack;
                this.tbStopSlack.Visibility = (this.Model.SlackRun) ? Visibility.Visible : Visibility.Collapsed;
                this.ucSlack.Visibility = (this.Model.SlackRun) ? Visibility.Visible : Visibility.Collapsed;
                this.btnSlack.Background = new SolidColorBrush((this.Model.SlackRun) ? ServiceBackground.Color : ApplicationBackground.Color);
            }

            if (e.PropertyName == nameof(this.Model.SettingsRun))
            {
                newValue = this.Model.SettingsRun;
                this.CollapseAllServices();
                this.tbCloseSettings.Visibility = (this.Model.SettingsRun) ? Visibility.Visible : Visibility.Collapsed;
                this.ucSettings.Visibility = (this.Model.SettingsRun) ? Visibility.Visible : Visibility.Collapsed;
                this.btnSettings.Background = new SolidColorBrush((this.Model.SettingsRun) ? ServiceBackground.Color : ApplicationBackground.Color);
            }

            //tbMessage.Visibility = Visibility.Collapsed;
            //grdUCs.Visibility = Visibility.Visible;
            grdUCs.Visibility = Visibility.Visible;
            if (!this.Model.SkypeRun &&
                !this.Model.WhatsappRun &&
                !this.Model.TelegramRun &&
                !this.Model.SlackRun &&
                !this.Model.SettingsRun)
            {
                grdUCs.Visibility = Visibility.Collapsed;
            }

            this.ppDashboard.Run = !newValue;

            //if (!newValue)
            //    tbMessage.Text = this.GetRandomWelcomeText();
        }
    }
}
