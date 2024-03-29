﻿using CefSharp;
using CefSharp.Wpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using UnifyMe.Core.Classes;
using UnifyMe.Core.Enums;
using UnifyMe.Core.Events;
using UnifyMe.Core.Models;
using UnifyMe.Core.UserPreferences;

namespace UnifyMe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EventsManager eventsManager;
        RegistryKey rk;
        public MainWindow()
        {
            InitializeCefSharp();
            InitializeObjects();
            InitializeComponent();
            this.Loaded += this.MainWindow_Loaded;
        }

        private void InitializeObjects()
        {
            this.eventsManager = new EventsManager();
        }

        private void InitializeCefSharp()
        {
            CefSettings settings = new CefSettings();
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            Cef.Initialize(settings);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Width = this.MinWidth;
            this.Height = this.MinHeight;
            this.rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            this.CheckUserPreferences();

            EventsManager.Instance.SubScribe(EventsNameEnums.OnSettingsRefresh, CheckUserPreferences);
            this.grdMain.Children.Add(new UserHome());
            //Geolocator gl = new Geolocator();
            //Geoposition gp = await gl.GetGeopositionAsync();
        }

        private void CheckUserPreferences()
        {
            bool HideTopBar = PreferencesManager.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "HideTopBar");
            bool LaunchFullScreen = PreferencesManager.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "LaunchFullScreen");
            bool ShowOnWindowsStartup = PreferencesManager.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "ShowOnWindowsStartup");
            ColorInfo ApplicationBackground = PreferencesManager.GetPropertyFromModel<SettingsModel, ColorInfo>(nameof(SettingsModel), "ApplicationBackground", new ColorInfo(color_name: "GreenYellow", color: Colors.GreenYellow));

            if (LaunchFullScreen)
                this.WindowState = System.Windows.WindowState.Maximized;
            else
                this.WindowState = System.Windows.WindowState.Normal;

            if (HideTopBar)
                this.WindowStyle = System.Windows.WindowStyle.None;
            else
                this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;

            if (ShowOnWindowsStartup)
                rk.SetValue("UnifyMe", Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "UnifyMe.exe"));
            else
                rk.DeleteValue("UnifyMe", false);
        }
    }
}
