using System.ComponentModel;

namespace UnifyMe.Core.Models
{
    public class ServicesContext : INotifyPropertyChanged
    {
        #region Private property
        private string _whatsappNotify;
        private string _telegramNotify;
        private string _skypeNotify;
        private string _slackNotify;
        private bool _whatsappRun;
        private bool _telegramRun;
        private bool _skypeRun;
        private bool _slackRun;
        private bool _settingsRun;
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        public string WhatsappNotify
        {
            get { return this._whatsappNotify; }
            set
            {
                this._whatsappNotify = value;
                this.OnPropertyChanged(nameof(this.WhatsappNotify));
            }
        }
        public string SlackNotify
        {
            get { return this._slackNotify; }
            set
            {
                this._slackNotify = value;
                this.OnPropertyChanged(nameof(this.SlackNotify));
            }
        }
        public bool WhatsappRun
        {
            get { return this._whatsappRun; }
            set
            {
                this._whatsappRun = value;
                this.OnPropertyChanged(nameof(this.WhatsappRun));
            }
        }
        public string TelegramNotify
        {
            get { return this._telegramNotify; }
            set
            {
                this._telegramNotify = value;
                this.OnPropertyChanged(nameof(this.TelegramNotify));
            }
        }
        public bool TelegramRun
        {
            get { return this._telegramRun; }
            set
            {
                this._telegramRun = value;
                this.OnPropertyChanged(nameof(this.TelegramRun));
            }
        }
        public string SkypeNotify
        {
            get { return this._skypeNotify; }
            set
            {
                this._skypeNotify = value;
                this.OnPropertyChanged(nameof(this.SkypeNotify));
            }
        }
        public bool SkypeRun
        {
            get { return this._skypeRun; }
            set
            {
                this._skypeRun = value;
                this.OnPropertyChanged(nameof(this.SkypeRun));
            }
        }
        public bool SlackRun
        {
            get { return this._slackRun; }
            set
            {
                this._slackRun = value;
                this.OnPropertyChanged(nameof(this.SlackRun));
            }
        }
        public bool SettingsRun
        {
            get { return this._settingsRun; }
            set
            {
                this._settingsRun = value;
                this.OnPropertyChanged(nameof(this.SettingsRun));
            }
        }

    }
}
