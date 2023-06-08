using System;
using UnifyMe.Core.Classes;
using UnifyMe.Core.Managers;

namespace UnifyMe.Core.Models
{
    public class SettingsModel : Model
    {

        private bool _minimizeOnClose;
        private bool _NotAllowNotify;
        private bool _hideTopBar;
        private bool _launchFullScreen;
        private bool _notUpdateNumMessages;
        private bool _clearSettings;
        private bool _allowUpdateNotify;
        private string _version;
        private string _lastUpdated;
        private bool _showOnWindowsStartup;
        private ColorInfo _serviceBackgroundActive;
        private ColorInfo _applicationBackground;
        private ColorInfo _fontColor;
        private bool _whatsappActive;
        private bool _telegramActive;
        private bool _slackActive;
        private bool _skypeActive;

        public bool MinimizeOnClose
        {
            get { return this._minimizeOnClose; }
            set
            {
                this._minimizeOnClose = value;
                this.OnPropertyChanged(nameof(this.MinimizeOnClose));
            }
        }

        public bool NotAllowNotify
        {
            get { return this._NotAllowNotify; }
            set
            {
                this._NotAllowNotify = value;
                this.OnPropertyChanged(nameof(this.NotAllowNotify));
            }
        }

        public bool HideTopBar
        {
            get { return this._hideTopBar; }
            set
            {
                this._hideTopBar = value;
                this.OnPropertyChanged(nameof(this.HideTopBar));
            }
        }

        public bool LaunchFullScreen
        {
            get { return this._launchFullScreen; }
            set
            {
                this._launchFullScreen = value;
                this.OnPropertyChanged(nameof(this.LaunchFullScreen));
            }
        }

        public bool NotUpdateNumMessages
        {
            get { return this._notUpdateNumMessages; }
            set
            {
                this._notUpdateNumMessages = value;
                this.OnPropertyChanged(nameof(this.NotUpdateNumMessages));
            }
        }

        public bool ClearSettings
        {
            get { return this._clearSettings; }
            set
            {
                this._clearSettings = value;
                this.OnPropertyChanged(nameof(this.ClearSettings));
            }
        }

        public bool AllowUpdateNotify
        {
            get { return this._allowUpdateNotify; }
            set
            {
                this._allowUpdateNotify = value;
                this.OnPropertyChanged(nameof(this.AllowUpdateNotify));
            }
        }

        public string Version
        {
            get
            {
                if (string.IsNullOrEmpty(this._version))
                    this._version = AppManager.GetAppVersion();

                return this._version;
            }
            set
            {
                this._version = value;
                this.OnPropertyChanged(nameof(this.Version));
            }
        }

        public string LastUpdated
        {
            get
            {
                if (string.IsNullOrEmpty(this._lastUpdated))
                    this._lastUpdated = DateTime.Now.Date.ToString();

                return this._lastUpdated;
            }
            set
            {
                this._lastUpdated = value;
                this.OnPropertyChanged(nameof(this.LastUpdated));
            }
        }

        public bool ShowOnWindowsStartup
        {
            get { return this._showOnWindowsStartup; }
            set
            {
                this._showOnWindowsStartup = value;
                this.OnPropertyChanged(nameof(this.ShowOnWindowsStartup));
            }
        }

        public ColorInfo ServiceBackgroundActive
        {
            get { return this._serviceBackgroundActive; }
            set
            {
                this._serviceBackgroundActive = value;
                this.OnPropertyChanged(nameof(this.ServiceBackgroundActive));
            }
        }

        public ColorInfo ApplicationBackground
        {
            get { return this._applicationBackground; }
            set
            {
                this._applicationBackground = value;
                this.OnPropertyChanged(nameof(this.ApplicationBackground));
            }
        }

        public ColorInfo FontColor
        {
            get { return this._fontColor; }
            set
            {
                this._fontColor = value;
                this.OnPropertyChanged(nameof(this.FontColor));
            }
        }

        public bool WhatsappActive
        {
            get { return this._whatsappActive; }
            set
            {
                this._whatsappActive = value;
                this.OnPropertyChanged(nameof(this.WhatsappActive));
            }
        }

        public bool TelegramActive
        {
            get { return this._telegramActive; }
            set
            {
                this._telegramActive = value;
                this.OnPropertyChanged(nameof(this.TelegramActive));
            }
        }

        public bool SlackActive
        {
            get { return this._slackActive; }
            set
            {
                this._slackActive = value;
                this.OnPropertyChanged(nameof(this.SlackActive));
            }
        }

        public bool SkypeActive
        {
            get { return this._skypeActive; }
            set
            {
                this._skypeActive = value;
                this.OnPropertyChanged(nameof(this.SkypeActive));
            }
        }
    }
}
