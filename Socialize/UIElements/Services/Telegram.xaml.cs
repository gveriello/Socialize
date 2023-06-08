using CefSharp.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using UnifyMe.Core.Enums;
using UnifyMe.Core.Models;
using UnifyMe.Core.UserPreferences;
using UnifyMe.Notification;

namespace UnifyMe.UIElements.Services
{
    public sealed partial class Telegram : UserControl
    {
        private ChromiumWebBrowser ucWebView;
        private static Telegram Instance;
        private int numMessagesToRead = 0;

        public Telegram()
        {
            Instance = this;
            this.InitializeComponent();
        }

        #region Suspend
        public static DependencyProperty SuspendProperty =
            DependencyProperty.Register("Suspend",
            typeof(bool),
            typeof(Telegram),
            new PropertyMetadata(default(bool), OnSuspendChanged));

        private static void OnSuspendChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue;
            if (value)
                Instance.SuspendService();
            else
                Instance.StartService();
        }

        public bool Suspend
        {
            get { return (bool)GetValue(SuspendProperty); }
            set { SetValue(SuspendProperty, value); }
        }
        #endregion

        #region Run
        public static DependencyProperty RunProperty =
            DependencyProperty.Register("Run",
            typeof(bool),
            typeof(Telegram),
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
        #endregion

        #region NumNotifications
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("NumNotifications",
            typeof(string),
            typeof(Telegram),
            new PropertyMetadata(default(string)));

        public string NumNotifications
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        #endregion

        private void StartService()
        {
            this.ucWebView = new ChromiumWebBrowser()
            {
                Address = "https://web.telegram.org"
            };
            this.tbMessage.Visibility = Visibility.Visible;
            this.ucWebView.Loaded += this.UcWebView_Loaded;
            this.ucWebView.TitleChanged += this.UcWebView_TitleChanged;

            if (this.grdBrowser.Children.Contains(this.ucWebView))
                this.grdBrowser.Children.Remove(this.ucWebView);

            this.grdBrowser.Children.Add(this.ucWebView);

            this.ucWebView.Visibility = Visibility.Visible;
            this.CheckNotify();
        }

        private void UcWebView_TitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.CheckNotify();
        }

        private void CheckNotify()
        {
            bool NotAllowNotify = Convert.ToBoolean(PreferencesManager.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "NotAllowNotify"));
            bool NotUpdateNumMessages = Convert.ToBoolean(PreferencesManager.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "NotUpdateNumMessages"));


            string titleWebView = null;
            int nMsgToRead = 0;

            if (NotUpdateNumMessages && NotAllowNotify)
                return;

            titleWebView = this.ucWebView?.Title;

            if (!string.IsNullOrEmpty(titleWebView))
                if (titleWebView.Contains("notific"))
                {
                    if (int.TryParse(titleWebView.Substring(0, titleWebView.IndexOf("notific") - 1), out nMsgToRead))
                    {
                        if (nMsgToRead > numMessagesToRead)
                        {
                            if (!NotAllowNotify)
                                Notificator.Services_SendNotify(ServicesEnums.Telegram);
                            numMessagesToRead = nMsgToRead;
                        }
                        else if (nMsgToRead <= numMessagesToRead)
                            numMessagesToRead = nMsgToRead;
                    }
                }
                else
                    numMessagesToRead = 0;

            if (!NotUpdateNumMessages)
            {
                string newHeader = string.Empty;
                if (numMessagesToRead > 0)
                    newHeader += $"({numMessagesToRead})";
                this.NumNotifications = newHeader;
            };
        }
        private void UcWebView_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbMessage.Visibility = Visibility.Collapsed;
            this.grdBrowser.Visibility = Visibility.Visible;
        }

        private void SuspendService()
        {
            this.grdBrowser.Visibility = Visibility.Collapsed;
        }

        private void StopService()
        {
            this.ucWebView.Loaded -= UcWebView_Loaded;
            this.ucWebView.TitleChanged -= UcWebView_TitleChanged;
            this.NumNotifications = null;
            this.tbMessage.Visibility = Visibility.Collapsed;
            this.grdBrowser.Visibility = Visibility.Collapsed;
        }
    }
}
