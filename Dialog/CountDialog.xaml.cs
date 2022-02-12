using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FlexConfirmMail.Dialog
{
    /// <summary>
    /// CountDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class CountDialog : Window
    {
        private int _timeout = 3;
        private DispatcherTimer _timer;

        public CountDialog()
        {
            InitializeComponent();
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
