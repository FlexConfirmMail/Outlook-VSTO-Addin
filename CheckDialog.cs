using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace CheckMyMail
{
    public partial class CheckDialog : Form
    {

        public CheckDialog()
        {
            InitializeComponent();
        }

        private string GetSortHash(Outlook.Recipient item)
        {
            int idx = item.Address.IndexOf('@');
            if (idx < 0)
            {
                return item.Address;
            }
            return item.Address.Substring(idx + 1) + item.Address.Substring(0, idx);
        }

        private List<Outlook.Recipient> GetSortedRecipients(Outlook.MailItem mail)
        {
            var list = new List<Outlook.Recipient>();
            foreach (Outlook.Recipient item in mail.Recipients)
            {
                list.Add(item);
            }

            list.Sort((Outlook.Recipient r1, Outlook.Recipient r2) => {
                return string.Compare(GetSortHash(r1), GetSortHash(r2));
            });

            return list;
        }

        public void LoadMail(Outlook.MailItem mail, Config config)
        {

            foreach (Outlook.Recipient item in GetSortedRecipients(mail))
            {
                int idx = item.Address.IndexOf('@');
                if (idx < 0 || config.TrustedDomains.Contains(item.Address.Substring(idx + 1)))
                {
                    clbTrusted.Items.Add(FormatAddress(item));
                }
                else
                {
                    clbExt.Items.Add(FormatAddress(item));
                }
            }

            foreach (Outlook.Attachment item in mail.Attachments)
            {
                clbFile.Items.Add(item.FileName);
            }
        }

        private string FormatAddress(Outlook.Recipient item)
        {
            string addr = item.Address;

            /* Handle LegacyExchangeDN */
            if (item.Address.IndexOf('@') < 0)
            {
                addr = $"組織アドレス ({item.Name})";
            }

            switch (item.Type)
            {
                case (int)Outlook.OlMailRecipientType.olBCC:
                    return $"Bcc : {addr}";
                case (int)Outlook.OlMailRecipientType.olCC:
                    return $" Cc : {addr}";
                default:
                    return $" To : {addr}";            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (AllChecked(clbExt) && AllChecked(clbTrusted) && AllChecked(clbFile))
            {
                btnOK.Enabled = true;
                btnOK.ForeColor = System.Drawing.Color.White;
                btnOK.BackColor = System.Drawing.Color.RoyalBlue;
            }
            else
            {
                btnOK.Enabled = false;
                btnOK.ForeColor = System.Drawing.Color.FromName("ControlText");
                btnOK.BackColor = System.Drawing.Color.FromName("Control");
            }
        }

        private bool AllChecked(CheckedListBox clb)
        {
            return clb.CheckedItems.Count == clb.Items.Count;
        }
    }
}
