using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace FlexConfirmMail.Dialog
{
    public partial class ConfirmNewDomainDialog : Window
    {
        public ConfirmNewDomainDialog()
        {
            QueueLogger.Log($"===== Open {nameof(ConfirmNewDomainDialog)} =====");
            InitializeComponent();
        }

        public ConfirmNewDomainDialog(HashSet<string> addresses) : this()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);

            if (addresses.Count > 2)
            {
                this.Height += (addresses.Count - 2) * textBlockBody.FontSize;
            }
            textBlockBody.Inlines.Add(Properties.Resources.ConfirmNewDomainsBody1);
            textBlockBody.Inlines.Add("\n\n");
            textBlockBody.Inlines.Add(new Run()
            {
                Text = string.Join("\n", addresses),
                FontWeight = FontWeights.Bold
            });
            textBlockBody.Inlines.Add("\n\n");
            textBlockBody.Inlines.Add(Properties.Resources.ConfirmNewDomainsBody2);
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            QueueLogger.Log($"* Send button clicked. closing...");
            DialogResult = true;
        }
    }
}