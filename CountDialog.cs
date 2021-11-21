using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckMyMail
{
    public partial class CountDialog : Form
    {
        public CountDialog()
        {
            InitializeComponent();
        }

        System.Threading.CancellationTokenSource source;
        Task task;

        private void CountDown(int n, System.Threading.CancellationToken token)
        {
            for (int i = 0; i < n; i++)
            {
                labelCount.Invoke(new Action(() => { labelCount.Text = $"{n - i}"; }));

                for (int j = 0; j < 10; j++)
                {
                    System.Threading.Thread.Sleep(100);
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }
            labelCount.Invoke(new Action(() => { labelCount.Text = "0";}));
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
            task = Task.Run(() => { CountDown(5, source.Token); }, source.Token);
        }

        private void CountDialog_Closing(object sender, EventArgs e)
        {
            source.Cancel();
            task.Wait();
        }
    }
}
