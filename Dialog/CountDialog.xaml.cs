using System;
using System.Windows;
using System.Windows.Threading;

namespace FlexConfirmMail.Dialog
{
    public partial class CountDialog : Window
    {
        private int _timeout;
        private DispatcherTimer _timer;

        public CountDialog()
        {
            InitializeComponent();
        }

        public CountDialog(Config config)
        {
            InitializeComponent();

            QueueLogger.Log("===== Open CountDialog() =====");

            _timeout = config.CountSeconds;

            if (!config.CountAllowSkip)
            {
                ButtonOK.Visibility = Visibility.Hidden;
                ButtonOK.IsEnabled = false;
            }

        }

        private void CountDialog_Loaded(object sender, EventArgs e)
        {
            labelCount.Content = _timeout;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            QueueLogger.Log($"* Timer activated. waiting for {_timeout} seconds");
        }

        private void CountDialog_Closing(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeout -= 1;

            // We need Dispatcher.Invoke() in order to modify
            // the UI from a thread.
            Dispatcher.Invoke(new Action(() =>
            {
                labelCount.Content = _timeout;
            }));

            if (_timeout <= 0)
            {
                QueueLogger.Log("* Timer finished. closing...");
                DialogResult = true;
                Close();
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            QueueLogger.Log("* Send button clicked. closing...");
            DialogResult = true;
        }
    }
}
