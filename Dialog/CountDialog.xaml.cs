using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using FlexConfirmMail.Config;

namespace FlexConfirmMail.Dialog
{
    /// <summary>
    /// CountDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class CountDialog : Window
    {
        private ConfigData _config;
        private int _timeout;
        private DispatcherTimer _timer;

        public CountDialog()
        {
            InitializeComponent();
        }

        public CountDialog(ConfigData config)
        {
            InitializeComponent();
            _config = config;
            Configure();
        }

        private void Configure()
        {
            _timeout = _config.GetInt("CountSeconds");
            if (!_config.GetBool("CountAllowSkip"))
            {
                buttonOK.Visibility = Visibility.Hidden;
                buttonOK.IsEnabled = false;
            }
        }

        private void HandleLoaded(object sender, EventArgs e)
        {
            labelCount.Content = _timeout;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += HandleTick;
            _timer.Start();
        }

        private void HandleClosing(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= HandleTick;
        }

        private void HandleTick(object sender, EventArgs e)
        {
            _timeout -= 1;

            Dispatcher.Invoke(new Action(() => {
                labelCount.Content = _timeout;
            }));

            if (_timeout <= 0)
            {
                DialogResult = true;
                Close();
            }
        }

        private void HandleClickOK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
