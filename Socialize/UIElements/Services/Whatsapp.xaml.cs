using CefSharp.Wpf;
using UnifyMe.Core.Enums;
using UnifyMe.Core.Models;
using UnifyMe.Core.UserPreferences;
using UnifyMe.Notification;
using System.Windows;
using System.Windows.Controls;

namespace UnifyMe.UIElements.Services
{
    public sealed partial class Whatsapp : UserControl
    {
        private ChromiumWebBrowser ucWebView;
        private static Whatsapp Instance;
        private int numMessagesToRead = 0;

        public Whatsapp()
        {
            Instance = this;
            this.InitializeComponent();
        }

        #region Run
        public static DependencyProperty RunProperty =
            DependencyProperty.Register("Run",
            typeof(bool),
            typeof(Whatsapp),
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
            typeof(Whatsapp),
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
                Address = "https://web.whatsapp.com"
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
            string titleWebView = null;
            int nMsgToRead = 0;

            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            //{
            //    try
            //    {
            //        //nascondo il riquadro che richiede l' abilitazione delle notifiche desktop
            //        var resultBoxNotify = await ucWebView.InvokeScriptAsync("eval",
            //    new string[] { "document.getElementsByClassName('_1Wk6A')[0].style.display = 'none';" });
            //        //boxNotifyIsCollapsed = resultBoxNotify == "none"; //se viene collassato, viene restituito none
            //    }
            //    catch { }
            //});
            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            //{
            //    try
            //    {
            //        //nascondo la label che indica che whatsapp è disponibile per desktop
            //        var resultLabelDesk = await ucWebView.InvokeScriptAsync("eval",
            //        new string[] { "document.getElementsByClassName('_3cK8a')[0].style.display = 'none';" });
            //        //labelWhatsappPC = resultLabelDesk == "none"; //se viene collassato, viene restituito none
            //    }
            //    catch { }
            //});


            bool NotAllowNotify = PreferencesManager.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "NotAllowNotify");
            bool NotUpdateNumMessages = PreferencesManager.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "NotUpdateNumMessages");

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
                                Notificator.Services_SendNotify(ServicesEnums.Whatsapp);
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
