using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlexConfirmMail.Dialog
{
    public partial class CountDialog : Form
    {
        public CountDialog()
        {
            InitializeComponent();
        }

        CancellationTokenSource source;
        Task task;

        private void CountDown(int seconds, CancellationToken token)
        {
            for (int i = 0; i < seconds; i++)
            {
                labelCount.Invoke(new Action(() => { labelCount.Text = $"{seconds - i}"; }));

                for (int j = 0; j < 10; j++)
                {
                    System.Threading.Thread.Sleep(100);
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }
            labelCount.Invoke(new Action(() => { labelCount.Text = "0"; }));
            System.Threading.Thread.Sleep(100);
            if (token.IsCancellationRequested)
            {
                return;
            }
            btnOK.Invoke(new System.Action(() => { btnOK.PerformClick(); }));
        }

        private void CountDialog_Shown(object sender, EventArgs e)
        {
            source = new System.Threading.CancellationTokenSource();
            task = Task.Run(() => { CountDown(3, source.Token); }, source.Token);
        }

        private void CountDialog_Closing(object sender, EventArgs e)
        {
            source.Cancel();
            task.Wait();
        }
    }
}
