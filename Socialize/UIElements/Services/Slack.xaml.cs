using CefSharp.Wpf;
using UnifyMe.Core.Models;
using UnifyMe.Core.UserPreferences;
using System.Windows;
using System.Windows.Controls;

namespace UnifyMe.UIElements.Services
{
    public sealed partial class Slack : UserControl
    {
        private ChromiumWebBrowser ucWebView;
        private static Slack Instance;
        //document.getElementsByClassName('p-channel_sidebar__badge c-mention_badge').length

        public Slack()
        {
            Instance = this;
            this.InitializeComponent();
        }

        #region Run
        public static DependencyProperty RunProperty =
            DependencyProperty.Register("Run",
            typeof(bool),
            typeof(Slack),
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
            typeof(Slack),
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
                Address = "https://slack.com/signin"
            };
            this.tbMessage.Visibility = Visibility.Visible;
            this.ucWebView.Loaded += this.UcWebView_Loaded;
            this.ucWebView.TitleChanged += this.UcWebView_TitleChanged;

            if (this.grdBrowser.Children.Contains(this.ucWebView))
                this.grdBrowser.Children.Remove(this.ucWebView);

            this.grdBrowser.Children.Add(this.ucWebView);
            this.ucWebView.Visibility = Visibility.Visible;
        }

        private void UcWebView_TitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool NotAllowNotify = ManagePreferences.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "NotAllowNotify");
            bool NotUpdateNumMessages = ManagePreferences.GetPropertyFromModel<SettingsModel, bool>(nameof(SettingsModel), "NotUpdateNumMessages");

            if (NotUpdateNumMessages && NotAllowNotify)
                return;

            int numMessagesToRead = 0;
            string titleWebView = null;
            int nMsgToRead = 0;

            titleWebView = this.ucWebView?.Title;

            //if (!string.IsNullOrEmpty(titleWebView))
            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            //{
            //    try
            //    {
            //        //recupero il numero di badge. per ogni chat non letta, viene assegnato un badge
            //        string[] script = { "document.getElementsByClassName('p-channel_sidebar__badge c-mention_badge').length.toString()" };
            //        string resultNumNotify = await ucWebView.InvokeScriptAsync("eval", script);

            //        if (int.TryParse(resultNumNotify, out nMsgToRead))
            //        {
            //            if (nMsgToRead > numMessagesToRead)
            //                if (!NotAllowNotify)
            //                    Notificator.Services_SendNotify(Notificator.Services.Slack);

            //            numMessagesToRead = nMsgToRead;
            //        }
            //    }
            //    catch { }
            //});

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

            if (this.grdBrowser.Children.Contains(this.ucWebView))
                this.grdBrowser.Children.Remove(this.ucWebView);
            this.ucWebView.Loaded -= UcWebView_Loaded;
            this.ucWebView.TitleChanged -= UcWebView_TitleChanged;
            this.NumNotifications = null;
            this.tbMessage.Visibility = Visibility.Collapsed;
            this.grdBrowser.Visibility = Visibility.Collapsed;
        }
    }
}
