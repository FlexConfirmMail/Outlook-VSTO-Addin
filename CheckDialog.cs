using System;
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
        public void LoadMail(Outlook.MailItem mail)
        {
            foreach (Outlook.Recipient item in mail.Recipients)
            {
                clbExt.Items.Add(DispAddress(item));
            }

            foreach (Outlook.Attachment item in mail.Attachments)
            {
                clbFile.Items.Add(item.FileName);
            }
        }
        private string DispAddress(Outlook.Recipient item)
        {
            switch (item.Type)
            {
                case (int)Outlook.OlMailRecipientType.olBCC:
                    return $"Bcc : {item.Address}";
                case (int)Outlook.OlMailRecipientType.olCC:
                    return $" Cc : {item.Address}";
                default:
                    return $" To : {item.Address}";            }
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
