using CefSharp.Wpf;
using UnifyMe.Core.Enums;
using UnifyMe.Core.Models;
using UnifyMe.Core.UserPreferences;
using UnifyMe.Notification;
using System.Windows;
using System.Windows.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UnifyMe.UIElements.Services
{
    public sealed partial class Skype : UserControl
    {
        private ChromiumWebBrowser ucWebView;
        private static Skype Instance;
        private int numMessagesToRead = 0;

        public Skype()
        {
            Instance = this;
            this.InitializeComponent();
            this.ucWebView = new ChromiumWebBrowser();
        }

        #region Run
        public static DependencyProperty RunProperty =
            DependencyProperty.Register("Run",
            typeof(bool),
            typeof(Skype),
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
            typeof(Skype),
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
                Address = "https://web.skype.com/"
            };
            this.tbMessage.Visibility = Visibility.Visible;
            this.ucWebView.Address = "https://web.skype.com/";
            this.ucWebView.Loaded += this.UcWebView_Loaded;
            this.ucWebView.TitleChanged += this.UcWebView_TitleChanged;

            if (!this.grdBrowser.Children.Contains(this.ucWebView))
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
            bool NotAllowNotify = PreferencesManager.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "NotAllowNotify");
            bool NotUpdateNumMessages = PreferencesManager.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "NotUpdateNumMessages");
            
            string titleWebView = null;
            int nMsgToRead = 0;

            if (NotUpdateNumMessages && NotAllowNotify)
                return;

            titleWebView = this.ucWebView?.Title;

            if (!string.IsNullOrEmpty(titleWebView))
                if (titleWebView.Contains("(") && titleWebView.Contains(")"))
                {
                    if (int.TryParse(titleWebView.Substring(titleWebView.IndexOf("(") + 1, titleWebView.IndexOf(")") - 1), out nMsgToRead))
                    {
                        if (nMsgToRead > numMessagesToRead)
                        {
                            if (!NotAllowNotify)
                                Notificator.Services_SendNotify(ServicesEnums.Skype);
                            numMessagesToRead = nMsgToRead;
                        }
                        else if (nMsgToRead <= numMessagesToRead)
                            numMessagesToRead = nMsgToRead;
                    }
                }

            if (!NotUpdateNumMessages)
            {
                string newHeader = string.Empty;
                if (numMessagesToRead > 0)
                    newHeader += $"({numMessagesToRead})";
                this.NumNotifications = newHeader;
            }
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
